using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QLcaulacbosinhvien.Models;
using QLcaulacbosinhvien.ViewModels;    
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace QLcaulacbosinhvien.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;

    public HomeController(ILogger<HomeController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }


    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Post()
    {
       var posts = (from Article in _context.Articles
                 join Account in _context.Accounts
                 on Article.AccountID equals Account.AccountID
                 where Article.IsShow == true
                
                 select new ArticleViewModel{
                       ArticleID = Article.ArticleID,
                     Title = Article.Title,
                     Content = Article.Content,
                     Image = Article.Image,
                     Date = Article.Date,
                     View = Article.View,
                     AuthorName = Account.UserName
                 }).OrderByDescending(p => p.ArticleID).Take(4).ToList();
        
        return View(posts);
    }
    [Route("/Home/post-{slug}-{id:long}.html", Name = "PostDetail")]
public IActionResult PostDetail(long? id)
{
    if (id == null)
    {
        return NotFound();
    }
    var post = _context.Articles.FirstOrDefault(m => (m.ArticleID == id) && (m.IsShow == true));
    if (post == null)
    {
        return NotFound();
    }
    _context.SaveChanges();

    return View(post);  // Ensure this matches the view expecting a single post
}
public IActionResult EventNotification()
{
    var userId = User.FindFirst("ID")?.Value;
    if (userId != null)
    {
        int accountId = int.Parse(userId);

        // Lấy danh sách sự kiện đang diễn ra mà người dùng đã tham gia, thực hiện trước trên server
        var events = _context.Events
            .Include(e => e.EventMembers)
            .ToList(); // Chuyển sang danh sách để xử lý điều kiện `IsActive` trên client

        // Lọc sự kiện dựa trên điều kiện `IsActive`
        var activeEvents = events
            .Where(e => e.IsActive && e.EventMembers.Any(em => em.AccountID == accountId))
            .ToList();

        // Kiểm tra xem có sự kiện nào mà người dùng chưa điểm danh không
        var unCheckedEvents = activeEvents
            .Where(e => e.EventMembers.Any(em => em.AccountID == accountId && !em.IsAttended))
            .ToList();

        if (!unCheckedEvents.Any())
        {
            // Nếu không có sự kiện nào chưa điểm danh, tắt thông báo
            return PartialView("EventNotification");
        }
        else
        {
            // Nếu có sự kiện chưa điểm danh, hiển thị thông báo
            return PartialView("EventNotification", unCheckedEvents);
        }
    }
    else
    {
        return NoContent(); 
    }
}




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
