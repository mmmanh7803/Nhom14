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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QLcaulacbosinhvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventsController : Controller
    {
          private readonly DataContext _context;

         public EventsController(DataContext context)
        {
            _context = context;
        }
  
public IActionResult Read(int page = 1, int pageSize = 3)
{
    var totalRecords = _context.Events.Count();
  var eventsToUpdate = _context.Events
    .Where(e => e.EndEvent < DateTime.Now && e.Status != false)
    .ToList();

foreach (var eventItem in eventsToUpdate)
{
    eventItem.Status = false;
}
_context.SaveChanges();
    var events = _context.Events
    .Include(a => a.Account)
        .OrderBy(a => a.EventID)
        .Skip((page - 1) * pageSize) // Bỏ qua số bản ghi trước trang hiện tại
        .Take(pageSize) // Lấy số bản ghi theo pageSize
        .ToList();
    
    // Tính tổng số trang
    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = totalPages;

    return View(events);
}
public IActionResult Create(){
         ViewBag.AccountList = _context.Accounts
        .Where(a => a.RoleID == 1 || a.RoleID == 2)
        .Select(a => new SelectListItem
        {
            Value = a.AccountID.ToString(),
            Text = a.UserName
        }).ToList();


    return View();
}

       [HttpPost]
        public IActionResult Create(EventViewModel model)
        {
    TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

    // Kiểm tra nếu thời gian bắt đầu là trong quá khứ
    DateTime vietnamEventStartDate = TimeZoneInfo.ConvertTimeToUtc(model.EventDate.Value, vietnamTimeZone);
    
    if (vietnamEventStartDate <= DateTime.UtcNow)
    {
        ModelState.AddModelError("EventDate", "Thời gian bắt đầu không thể là quá khứ.");
    }

    // Kiểm tra nếu thời gian kết thúc nhỏ hơn thời gian bắt đầu
    DateTime vietnamEventEndDate = TimeZoneInfo.ConvertTimeToUtc(model.EndEvent.Value, vietnamTimeZone);

    if (vietnamEventEndDate <= vietnamEventStartDate)
    {
        ModelState.AddModelError("EndEvent", "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");
    }
     bool isOverlap = _context.Events
        .Any(e => e.EventDate < model.EndEvent && model.EventDate < e.EndEvent);
    
    if (isOverlap)
    {
        ModelState.AddModelError("", "Thời gian sự kiện bị trùng với một sự kiện khác.");
    }

            if (ModelState.IsValid)
            {
                try
                {
            // Create the Article object with user-specific data
            var events = new Event
        {
        
            EventName = model.EventName,
            EventDescription = model.EventDescription,
            Images = model.Images,
            EventDate = model.EventDate,
            EndEvent = model.EndEvent,
            EventLocation = model.EventLocation,
            Point = model.Point,
            Status = model.Status,
            AccountID = model.AccountID,
            MemberJoin = model.MemberJoin
          
        };

            // Save the new article to the database
            _context.Events.Add(events);
            _context.SaveChanges();
    var member = _context.Accounts
    .FirstOrDefault(a => a.AccountID == model.AccountID);
            var eventMember = new EventMember
            {
                EventID = events.EventID,
                AccountID = model.AccountID,
                MemberID = member.MemberID // Cột AccountID hoặc tên cột chính xác
            };
            _context.EventMembers.Add(eventMember);
            _context.SaveChanges();

            // Redirect to a success or "read" page after saving
            return RedirectToAction("Read");
        }
        catch (Exception ex)
        {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
    ModelState.AddModelError("", $"Lỗi khi lưu sự kiện: {errorMessage}");
        }
    }
if (!ModelState.IsValid)
    {
        ViewBag.AccountList = _context.Accounts
            .Where(a => a.RoleID == 1 || a.RoleID == 2) // Điều kiện RoleID
            .Select(a => new SelectListItem
            {
                Value = a.AccountID.ToString(),
                Text = a.UserName
            }).ToList();
        return View(model);
    }
    return View(model);
}


    public IActionResult Edit(int id){
           ViewBag.AccountList = _context.Accounts
        .Where(a => a.RoleID == 1 || a.RoleID == 2)
        .Select(a => new SelectListItem
        {
            Value = a.AccountID.ToString(),
            Text = a.UserName
        }).ToList();
        var events = _context.Events.FirstOrDefault(e => e.EventID == id);
       
        if(events == null) return NotFound();
        var model = new EventViewModel
        {
            EventID = events.EventID,
            EventName = events.EventName,
            EventDescription = events.EventDescription,
            Images = events.Images,
            EventDate = events.EventDate,
            EndEvent = events.EndEvent,
            EventLocation = events.EventLocation,
            Point = events.Point,
            Status = events.Status,
            AccountID = events.AccountID,
            MemberJoin = events.MemberJoin
          
             
        };
        return View(model);
    }
   

[HttpPost]
public IActionResult Edit(EventViewModel model)
{
    if (ModelState.IsValid)
    {
        var events = _context.Events.FirstOrDefault(e => e.EventID == model.EventID);
        if (events != null){
           
        // Cập nhật thông tin sự kiện bao gồm AccountID mới từ form
        events.EventName = model.EventName;
        events.EventDescription = model.EventDescription;
        events.EventDate = model.EventDate;
        events.EndEvent = model.EndEvent;
        events.EventLocation = model.EventLocation;
        events.Images = model.Images;
        events.Point = model.Point;
        events.Status = model.Status;
        // Cập nhật AccountID với người quản lý mới
        events.AccountID = model.AccountID;
         events.MemberJoin = model.MemberJoin;
        
         _context.Events.Update(events);
         _context.SaveChanges();
        } 
        return RedirectToAction("Read");
    }

    return View(model);
}
 [HttpPost]
        public IActionResult Delete(int id)
        {
            var delmenu = _context.Events.Find(id);
            if (delmenu != null)
            {
                _context.Events.Remove(delmenu);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }      
  public IActionResult UpdateIsShow(int id, bool isActive)
        {
            try
            {
                var menuItem = _context.Events.Find(id);

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