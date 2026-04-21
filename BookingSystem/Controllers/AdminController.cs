using Microsoft.AspNetCore.Mvc;
using BookingSystem.Data;
using System;
using System.Linq;

namespace BookingSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly BookingSystemContext _context;

        public AdminController(BookingSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminLogin(string username, string password, int passkey)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError("", "Username is required.");
                return View("Index");
            }

            if (passkey <= 0)
            {
                ModelState.AddModelError("", "A valid passkey is required.");
                return View("Index");
            }

            var user = _context.Admins
                .FirstOrDefault(a => a.AdminUsername == username && a.AdminPasskey == passkey);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid admin login details.");
                return View("Index");
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetInt32("AdminId", user.AdminId);
            HttpContext.Session.SetString("UserName", user.AdminUsername);
            HttpContext.Session.SetString("UserRole", "Admin");

            return RedirectToAction("Index", "Statistics");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Admin");
        }
    }
}