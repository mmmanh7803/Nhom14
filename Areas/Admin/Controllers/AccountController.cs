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
            public IActionResult Read(int page = 1, int pageSize = 3)
{
    var totalRecords = _context.Accounts.Count();
    var account = _context.Accounts
        .OrderBy(a => a.AccountID)
        .Skip((page - 1) * pageSize) // Bỏ qua số bản ghi trước trang hiện tại
        .Take(pageSize) // Lấy số bản ghi theo pageSize
        .ToList();

    // Tính tổng số trang
    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = totalPages;

    return View(account);
}
 
        public IActionResult Create(){

            return View();
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
       public IActionResult Create(Account model)
        {
            if (ModelState.IsValid)
            {
            var existingUser = _context.Accounts.SingleOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }
             var member = _context.Members.SingleOrDefault(m => m.MemberEmail == model.Email);
             if(member == null){
                 ModelState.AddModelError("Email", "Không phải là Email thành viên trong CLB");
                return View(model);
             }
            // Tạo tài khoản mới
            var user = new Account
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password, // Băm mật khẩu trong thực tế
                MemberID = member.MemberID,
                RoleID = model.RoleID // Role mặc định cho user mới
            };

            _context.Accounts.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Read");
            }
            return View(model);

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
		public IActionResult Edit(Account model)
		{
			if (ModelState.IsValid)
			{ 
                var existingUser = _context.Accounts.SingleOrDefault(u => u.Email == model.Email);

                if (existingUser != null)
                {
                    existingUser.UserName = model.UserName;
                    existingUser.Email = model.Email;
                    existingUser.Password = model.Password; 
                    existingUser.RoleID = model.RoleID;
            
                    _context.SaveChanges();
                }
                 return RedirectToAction("Read");
			}
			return View(model);
		}

    }
}