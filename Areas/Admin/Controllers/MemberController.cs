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
     [Authorize(Roles = "1")]
    [Area("Admin")]
    public class MemberController : Controller
    {
         private readonly DataContext _context;

         public MemberController(DataContext context)
        {
            _context = context;
        }
  
    public IActionResult Read(int page = 1, int pageSize = 3)
{
    var totalRecords = _context.Members.Count();
    var member = _context.Members
        .OrderBy(a => a.MemberID)
        .Skip((page - 1) * pageSize) // Bỏ qua số bản ghi trước trang hiện tại
        .Take(pageSize) // Lấy số bản ghi theo pageSize
        .ToList();

    // Tính tổng số trang
    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = totalPages;

    return View(member);
}
     public IActionResult Create()
		{
          return View();
		}

        [HttpPost]

       public IActionResult Create(Member member)
        {
           if (ModelState.IsValid)
    {
        // Kiểm tra xem email hoặc mã sinh viên đã tồn tại chưa
        var existingMember = _context.Members
            .FirstOrDefault(m => m.MemberEmail == member.MemberEmail || m.MemberMSV == member.MemberMSV);
        if (existingMember != null)
        {
            ModelState.AddModelError("", "Thành viên với email hoặc mã số sinh viên đã tồn tại.");
            return View(member);
        }

        // Gán ngày tham gia và các giá trị khác nếu cần
        member.JoinDate = DateTime.Now;
        member.Point = 0; // Điểm mặc định là 0 khi mới thêm
        member.Status = true; // Kích hoạt trạng thái hoạt động của thành viên

        // Lưu thành viên vào cơ sở dữ liệu
        _context.Members.Add(member);
        _context.SaveChanges();

        return RedirectToAction("Read"); // Chuyển hướng tới danh sách thành viên hoặc trang khác
    }
       return View(member);

    }
 public IActionResult Edit(int id)
{
    var member = _context.Members.Find(id); 

    if (member == null)
    {
        // Debugging log
        throw new Exception("Member object is null.");
    }

    return View(member);
}
       [HttpPost]
		[ValidateAntiForgeryToken]
	public IActionResult Edit(Member model)
		{
			if (ModelState.IsValid)
			{ 
                var member = _context.Members.Find(model.MemberID);
                if (member != null)
                {
                    member.MemberName = model.MemberName;
                    member.MemberEmail = model.MemberEmail;
                    member.MemberPhone = model.MemberPhone;
                    member.MemberMSV = model.MemberMSV;
                    member.Point = model.Point;
                    member.Images = model.Images;
              

                    _context.SaveChanges();
                }
                 return RedirectToAction("Read");
			}
			return View(model);
		}
            
    [HttpPost]
        public IActionResult Delete(int id)
        {
            var delmenu = _context.Members.Find(id);
            if (delmenu != null)
            {
                _context.Members.Remove(delmenu);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }      
  public IActionResult UpdateIsShow(int id, bool isActive)
        {
            try
            {
                var menuItem = _context.Members.Find(id);

                if (menuItem != null)
                {
                    menuItem.Status = isActive;
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
    }
}