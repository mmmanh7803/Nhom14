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
    public class StatisticController : Controller
    {
         private readonly DataContext _context;

         public StatisticController(DataContext context)
        {
            _context = context;
        }
  
    public IActionResult Read()
{
    
    return View();
}
public IActionResult PastEvents()
{
    var events = _context.Events
        .Where(e => e.EndEvent < DateTime.Now)
        .Select(e => new
        {
            e.EventID,
            e.EventName
        })
        .ToList();

    return Json(events); // Return past events as JSON
}

public IActionResult AttendanceStats(int eventId)
{
    var stats = _context.Events
        .Where(e => e.EventID == eventId)
        .Select(e => new
        {
            e.EventName,
            Attended = e.EventMembers.Count(em => em.IsAttended),
            Absent = e.EventMembers.Count(em => !em.IsAttended)
        })
        .FirstOrDefault();

    return Json(stats); // Return attendance stats
}


public IActionResult GetAttendanceStatistics(int eventId)
{
    var eventData = _context.Events
        .Where(e => e.EventID == eventId)
        .Select(e => new
        {
            EventName = e.EventName,
            AttendanceCount = e.EventMembers.Count(em => em.IsAttended), // Số lượng sinh viên đã điểm danh
            AbsenceCount = e.EventMembers.Count(em => !em.IsAttended) // Số lượng sinh viên chưa điểm danh
        })
        .FirstOrDefault();

    return Json(eventData); // Trả về dữ liệu điểm danh của sự kiện
}

    }
}