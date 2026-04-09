using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Data;
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
        public IActionResult AdminLogin(String username, string password, int passkey)
        {
            var user = _context.Admins
                .FirstOrDefault(a => a.AdminUsername == username && a.AdminPasskey == passkey);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View("Index");
            }
            
            // Store user in session
            HttpContext.Session.SetString("UserName", user.AdminUsername);
            //HttpContext.Session.SetString("Password", user.Password);
            //HttpContext.Session.SetInt32("AdminPasskey", user.AdminPasskey);
            
            return RedirectToAction("Index", "Statistics");
        }
        
    }
}
