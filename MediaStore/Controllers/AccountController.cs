using System.Security.Claims;
using MediaStore.Data;
using MediaStore.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AimsContext _context;
        public AccountController(AimsContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string loginAsGuest)
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
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Login Failed";
                return View();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserID", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookieAuth", principal);

            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
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