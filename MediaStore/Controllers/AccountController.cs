using System.Security.Claims;
using MediaStore.Data;
using MediaStore.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace MediaStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AimsContext db;
        public AccountController(AimsContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login(string? returnUrl = null) => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string loginAsGuest, string? returnUrl)
        {
            if (loginAsGuest == "true")
            {
                var guestClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Guest"),
                    new Claim(ClaimTypes.Role, "Guest")
                };
                var guestIdentity = new ClaimsIdentity(guestClaims, "MyCookieAuth");
                var guestPrincipal = new ClaimsPrincipal(guestIdentity);
                await HttpContext.SignInAsync("MyCookieAuth", guestPrincipal);
                return RedirectToAction("Index", "Home");
            }
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Login Failed";
                return View();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username), // Ten nguoi dung
                new Claim("UserID", user.UserId.ToString()), // ID neu can
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Customer"),
                new Claim(ClaimTypes.Email, user.Email) // Email nguoi dung => loc don hang
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookieAuth", principal);

            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

    }
}
