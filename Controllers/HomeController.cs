using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QLcaulacbosinhvien.Models;
using QLcaulacbosinhvien.ViewModels;    

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
    [Route("/post-{slug}-{id:long}.html", Name = "PostDetail")]
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
