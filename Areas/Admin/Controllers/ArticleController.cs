using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using QLcaulacbosinhvien.Models;
using QLcaulacbosinhvien.ViewModels;
namespace QLcaulacbosinhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
           private readonly DataContext _context;
        public ArticleController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Read()
        {
            var posts = (from Article in _context.Aricles
                 join Member in _context.Members
                 on Article.MemberID equals Member.MemberID
                 where Article.Status == true
                
                 select new ArticleViewModel{
                       ArticleID = Article.ArticleID,
                     Title = Article.Title,
                     Content = Article.Content,
                     Image = Article.Image,
                     Date = Article.Date,
                     View = Article.View,
                     AuthorName = Member.MemberName
                 }).OrderByDescending(p => p.ArticleID).Take(4).ToList();
        
        return View(posts);
        }
    }
}