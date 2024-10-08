using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QLcaulacbosinhvien.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using QLcaulacbosinhvien.ViewModels;    

namespace QLcaulacbosinhvien.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly DataContext _context;

    public AccountController(ILogger<AccountController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

      #region Login

    [HttpGet]
    public IActionResult Login(string? ReturnUrl)
    {
        ViewBag.ReturnUrl = ReturnUrl;
			return View();
    }
    public IActionResult Register()
    {
        return View();
    }
        [HttpPost]
public async Task<IActionResult> Login(Account model, string? ReturnUrl)
{
    ViewBag.ReturnUrl = ReturnUrl;
    if (ModelState.IsValid)
    {
        var Userr = _context.Accounts.SingleOrDefault(p => p.Email == model.Email);
        if (Userr == null)
        {
            ModelState.AddModelError("loi", "Không có khách hàng này");
        }
        else
        {
            if (Userr.Password != model.Password)
            {
                ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
            }
            else
            {
                // Tạo claims
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Email, Userr.Email),
                    new Claim("ID", Userr.AccountID.ToString()),
                    new Claim(ClaimTypes.Name, Userr.UserName)
                };

                // Thêm role dựa trên RoleID của người dùng
                if (Userr.RoleID == 1)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "1")); // Admin
                }
                else if (Userr.RoleID == 2)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "2")); // Club Leader
                }
                else if (Userr.RoleID == 3)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "3")); // Member
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Đăng nhập
                await HttpContext.SignInAsync(claimsPrincipal);

                // Điều hướng dựa trên role
                if (Userr.RoleID == 1 || Userr.RoleID == 2)
                {
                    return RedirectToAction("Index", "Admin"); // Điều hướng đến trang admin nếu là admin hoặc club leader
                }
                else if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }

    return View();
}

    	#endregion
[HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _context.Accounts.SingleOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return View(model);
            }

            var user = new Account
            {
                Email = model.Email,
                Password = model.Password, // Ideally, you should hash the password before storing it
                UserName = model.UserName,
                RoleID = 3 // Default role for new users
            };

            _context.Accounts.Add(user);
            await _context.SaveChangesAsync();

            // Log the user in after registration
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("ID", user.AccountID.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "2")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }


		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}

         [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}