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
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Claims;




namespace QLcaulacbosinhvien.Areas.Admin.Controllers
{
    
    [Authorize(Roles = "1,2")]
    [Area("Admin")]
    public class ArticleController : Controller
    {
           private readonly DataContext _context;
           private readonly IWebHostEnvironment _env;
   public ArticleController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

 public IActionResult Read(DateTime? startDate, DateTime? endDate, int page = 1)
{
    int pageSize = 3;  

    var postsQuery = _context.Articles.AsQueryable();


    if (startDate.HasValue && endDate.HasValue)
    {
        postsQuery = postsQuery.Where(a => a.Date >= startDate && a.Date <= endDate);
    }

    // Fetch total count for pagination
    int totalPosts = postsQuery.Count();

    var posts = postsQuery
        .OrderByDescending(a => a.Date)  
        .Skip((page - 1) * pageSize)            
        .Take(pageSize)                         
        .ToList();

    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);
    ViewBag.StartDate = startDate;
    ViewBag.EndDate = endDate;

    return View(posts);
}

        public IActionResult Create()
		{
          return View();
		}

        [HttpPost]
        public IActionResult Create(Article model)
        {
            if (ModelState.IsValid)
            {
                try
                {
            // Retrieve the current user's ID from claims
            var userId = User.FindFirst("ID")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                // If the userId is null or empty, return an error message
                ModelState.AddModelError("", "Unable to find user ID.");
                return View(model);
            }

            // Create the Article object with user-specific data
            var article = new Article
            {
                Title = model.Title,
                Content = model.Content,
                Image = model.Image,
                Date = DateTime.Now,
                View = 0,
                AuthorName=model.AuthorName,
                IsShow = model.IsShow,
                AccountID = int.Parse(userId),  // Ensure userId can be parsed to an integer
                Description = model.Description
            };

            // Save the new article to the database
            _context.Articles.Add(article);
            _context.SaveChanges();

            // Redirect to a success or "read" page after saving
            return RedirectToAction("Read");
        }
        catch (Exception ex)
        {
            // Handle and log the exception if something goes wrong
            ModelState.AddModelError("", $"An error occurred while creating the article: {ex.Message}");
        }
    }

    // If the model state is invalid, return the model back to the view with validation messages
    return View(model);
}

      public IActionResult Edit(int id)
{
    
    var article = _context.Articles.Find(id);
    if (article == null)
    {
        return NotFound();
    }

    return View(article);
}
	    [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Article model)
		{
			if (ModelState.IsValid)
			{ 
                var article = _context.Articles.Find(model.ArticleID);
                if (article != null)
                {
                    article.Title = model.Title;
                    article.Description = model.Description;
                    article.Image = model.Image; 
                    article.AuthorName = model.AuthorName;
                    article.Content = model.Content;

                    _context.SaveChanges();
                }
                 return RedirectToAction("Read");
			}
			return View(model);
		}

    [HttpPost]
        public IActionResult Delete(int id)
        {
            var delmenu = _context.Articles.Find(id);
            if (delmenu != null)
            {
                _context.Articles.Remove(delmenu);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }      
  public IActionResult UpdateIsShow(int id, bool isActive)
        {
            try
            {
                var menuItem = _context.Articles.Find(id);

                if (menuItem != null)
                {
                    menuItem.IsShow = isActive;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Update successful" });
                }

                return Json(new { success = false, message = "Menu item not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            
        }
     [HttpPost]
public async Task<IActionResult> UploadImage(IFormFile upload)
{
    if (upload != null && upload.Length > 0)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "/images", upload.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await upload.CopyToAsync(stream);
        }

        // Trả về URL của file để hiển thị trong CKEditor
        return Json(new { url = "/images/" + upload.FileName });
    }

    return Json(new { error = "Không có file nào được chọn." });
}

    }    

}
