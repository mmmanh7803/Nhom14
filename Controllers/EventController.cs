using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QLcaulacbosinhvien.Models;
using QLcaulacbosinhvien.ViewModels;    
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using QRCoder;
using System.Drawing.Design;

namespace QLcaulacbosinhvien.Controllers;

public class EventController : Controller
{
    private readonly DataContext _context;

    public EventController(DataContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;
        var events = _context.Events
            .Include(a => a.EventMembers) 
            .Where(e => e.Status == true)
            .ToList();

        return View(events);
    }

    [HttpPost]
    public JsonResult Participate(int eventId)
    {
        try
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để tham gia sự kiện." });
            }

            var userId = User.FindFirst("ID")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Không thể lấy thông tin người dùng." });
            }

            var accountId = int.Parse(userId);
            var member = _context.Accounts.FirstOrDefault(a => a.AccountID == accountId);

            if (member == null)
            {
                return Json(new { success = false, message = "Không tìm thấy tài khoản." });
            }

            var isJoined = _context.EventMembers.Any(em => em.EventID == eventId && em.AccountID == accountId && em.MemberID == member.MemberID);

            if (isJoined)
            {
                return Json(new { success = false, message = "Bạn đã tham gia sự kiện này." });
            }

            var eventMember = new EventMember
            {
                EventID = eventId,
                AccountID = accountId,
                MemberID = member.MemberID,
                IsAttended = false
            };

            _context.EventMembers.Add(eventMember);
            _context.SaveChanges();

            return Json(new { success = true, message = "Bạn đã tham gia thành công!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Json(new { success = false, message = "Có lỗi xảy ra khi tham gia sự kiện." });
        }
    }

public async Task<IActionResult> GetAttendanceQRCode(int accountId)
{
    // Lấy userId từ Claims
    var userId = User.FindFirst("ID")?.Value;
    accountId = int.Parse(userId);

    // Lấy danh sách sự kiện đang diễn ra mà người dùng đã tham gia
    var ongoingEvents = await _context.Events
        .Include(e => e.EventMembers)
        .Where(e => e.EventDate <= DateTime.Now && e.EndEvent >= DateTime.Now &&
                    e.EventMembers.Any(em => em.AccountID == accountId))
        .ToListAsync();

    if (!ongoingEvents.Any())
    {
        return NotFound("Không tìm thấy sự kiện đang diễn ra cho tài khoản này.");
    }

    var eventIdToUse = ongoingEvents.First().EventID;

    // Tạo URL mã QR dẫn đến `AttendEvent`
    var ngrokUrl = "https://c337-2401-d800-73c0-272-a5bc-90c6-7225-551.ngrok-free.app";
    var qrCodeUrl = ngrokUrl + Url.Action("AttendEvent", "Event", new { accountId, eventId = eventIdToUse });
    
    // Tạo mã QR và lưu vào URL
    using (var qrGenerator = new QRCodeGenerator())
    using (var qrCodeData = qrGenerator.CreateQrCode(qrCodeUrl, QRCodeGenerator.ECCLevel.Q))
    using (var qrCode = new QRCode(qrCodeData))
    {
        using (var qrCodeImage = qrCode.GetGraphic(20))
        {
            using (var ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var base64String = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                
                ViewBag.EventID = eventIdToUse;
                return PartialView("QRCodePartial", base64String); // Trả về mã QR dưới dạng base64
            }
        }
    }
}


    [HttpGet]
    public IActionResult AttendEvent(int accountId, int eventId)
    {
        var eventMember = _context.EventMembers
            .FirstOrDefault(em => em.AccountID == accountId && em.EventID == eventId);

        if (eventMember == null)
        {
            return Json(new { success = false, message = "Bạn chưa tham gia sự kiện này." });
        }

        if (eventMember.IsAttended)
        {
            return Json(new { success = false, message = "Bạn đã điểm danh trước đó." });
        }

        eventMember.IsAttended = true;
        _context.SaveChanges();

        return Json(new { success = true, message = "Điểm danh thành công!" });
    }
}
