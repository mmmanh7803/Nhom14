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

namespace QLcaulacbosinhvien.Areas.Admin.Controllers
{   
    [Authorize(Roles = "1")]
    [Area("Admin")]
    public class AccountController : Controller
    { 
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Read()
        {
            var list = _context.Accounts.OrderBy(m => m.AccountID).ToList();
            return View(list);
        }
        public IActionResult Create(){

            return View();
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
       public IActionResult Create(Account mn)
        {
            if (ModelState.IsValid)
            {
                _context.Accounts.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Read");
            }
            return View(mn);

        }
        [HttpPost]   
         public IActionResult Delete(int id)
        {
            var delmenu = _context.Accounts.Find(id);
            if (delmenu != null)
            {
                _context.Accounts.Remove(delmenu);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }
public IActionResult Edit(int id)
{
    // Truy xuất tài khoản theo id từ cơ sở dữ liệu
    var account = _context.Accounts.Find(id);
    
    // Kiểm tra nếu tài khoản không tồn tại
    if (account == null)
    {
        return NotFound(); // Hoặc có thể trả về một thông báo lỗi thích hợp
    }

    return View(account); // Trả về view với model tài khoản
}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Account mn)
		{
			if (ModelState.IsValid)
			{
				_context.Accounts.Update(mn);
				_context.SaveChanges();
				return RedirectToAction("Read");
			}
			return View(mn);
		}
    }
}