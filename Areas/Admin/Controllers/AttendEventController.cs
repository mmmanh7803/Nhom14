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
using Microsoft.AspNetCore.Mvc.Rendering;       
using X.PagedList.Extensions;



namespace QLcaulacbosinhvien.Areas.Admin.Controllers;
 [Area("Admin")]
    public class AttendEventController : Controller
    {
          private readonly DataContext _context;

         public AttendEventController(DataContext context)
        {
            _context = context;
        }
public IActionResult Read(int eventId, int page = 1)
{
    if (_context == null || _context.EventMembers == null)
    {
        return NotFound("Database context or EventMembers table is not available.");
    }

    int pageSize = 3;
    var Query = _context.EventMembers.Include(em => em.Member).AsQueryable();

    if (eventId != 0)
    {
        Query = Query.Where(em => em.EventID == eventId);
    }

    int totalPosts = Query.Count();
    if (totalPosts == 0)
    {
        return NotFound("No members found for the given event ID.");
    }

    var member = Query
        .OrderByDescending(a => a.EventID)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    int totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

    ViewBag.EventId = eventId;
    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = totalPages;
    ViewBag.Events = _context.Events?.ToList() ?? new List<Event>();

    return View(member);
}
    [HttpPost]
public IActionResult Delete(int id)
{
    var member = _context.EventMembers.Find(id);
    if (member != null)
    {
        _context.EventMembers.Remove(member);
        _context.SaveChanges();
        return Json(new { success = true });
    }
    return Json(new { success = false });
}
[HttpPost]
[HttpPost]
public IActionResult UpdateAttendance(int id, bool isAttended)
{
    var eventMember = _context.EventMembers.FirstOrDefault(em => em.EventAccountID == id);
    if (eventMember != null)
    {
        eventMember.IsAttended = isAttended;
        _context.SaveChanges();
        return Json(new { success = true });
    }
    return Json(new { success = false });
}




    }
