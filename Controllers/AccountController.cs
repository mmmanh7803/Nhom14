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


    // Phương thức POST để xử lý đăng nhập
 [HttpPost]
public async Task<IActionResult> Login(Account model, string? ReturnUrl)
	{
			ViewBag.ReturnUrl = ReturnUrl;
			if (ModelState.IsValid)
			{
			var Userr = _context.Accounts.Include(a => a.member).FirstOrDefault(u => u.Email == model.Email);

				if (Userr == null || Userr.MemberID == 0 || Userr.member == null)
                {
                    ModelState.AddModelError("loi", "Tài khoản không hợp lệ hoặc không có thành viên liên kết.");
                    return View(model);
                }
				else
				{
				
						if (Userr.Password != model.Password)
                        {
                            ModelState.AddModelError("loi", "Sai mật khẩu.");
                            return View(model);
                        }
                        else
                        {
                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, Userr.Email),
                                new Claim("ID", Userr.RoleID.ToString()),
                                new Claim(ClaimTypes.Name, Userr.UserName),

                                //claim - role động
                                new Claim(ClaimTypes.Role, "2"),
                                new Claim(ClaimTypes.Role, "1")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                if (Userr.RoleID == 1 || Userr.RoleID == 2)
                {
                    return RedirectToAction("Index", "Admin"); // Điều hướng đến trang admin
                }
                else if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return Redirect("/");
                }
                        }
				}
		    }

			return View();
		}
    #endregion


     [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // [POST] /Account/Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingUser = _context.Accounts.SingleOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }
            var member = _context.Members.SingleOrDefault(m => m.MemberEmail == model.Email);
            // Tạo tài khoản mới
            var user = new Account
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password, // Băm mật khẩu trong thực tế
                MemberID = member.MemberID,
                RoleID = 3 // Role mặc định cho user mới
            };

            _context.Accounts.Add(user);
            await _context.SaveChangesAsync();

            // Tự động đăng nhập sau khi đăng ký
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("ID", user.AccountID.ToString()),
                new Claim(ClaimTypes.Role, user.RoleID.ToString())
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