using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLcaulacbosinhvien.Models;

namespace QLcaulacbosinhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMenuController : Controller
    {
   private readonly DataContext _context;
        public AdminMenuController(DataContext context)
        {
            _context = context;
        }

       public IActionResult read()
        {
            var list = _context.Menus.OrderBy(m => m.MenuID).ToList();
            return View(list);
        }
        public IActionResult Create()
        {
            var mnList = (from m in _context.Menus
                          select new SelectListItem()
                          {
                              Text = m.MenuName,
                              Value = m.MenuID.ToString(),
                          }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Menu mn)
        {
            if (ModelState.IsValid)
            {
                _context.Menus.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("read");
            }
            return View(mn);
        }
        [HttpPost]
           public IActionResult Delete(int id)
        {
            var delmenu = _context.Menus.Find(id);
            if (delmenu != null)
            {
                _context.Menus.Remove(delmenu);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }
	public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var mn = _context.Menus.Find(id);
			if (mn == null)
			{
				return NotFound();
			}
			var mnList = (from m in _context.Menus
						  select new SelectListItem()
						  {
							  Text = m.MenuName,
							  Value = m.MenuID.ToString(),
						  }).ToList();
			mnList.Insert(0, new SelectListItem()
			{
				Text = "----Select----",
				Value = "0"
			});
			ViewBag.mnList = mnList;
			return View(mn);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Menu mn)
		{
			if (ModelState.IsValid)
			{
				_context.Menus.Update(mn);
				_context.SaveChanges();
				return RedirectToAction("read");
			}
			return View(mn);
		}
		[HttpPost]
        public IActionResult UpdateIsActive(int id, bool isActive)
        {
            try
            {
                var menuItem = _context.Menus.Find(id);

                if (menuItem != null)
                {
                    menuItem.IsActive = isActive;
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}