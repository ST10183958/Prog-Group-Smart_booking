using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Data;
using System.Security.Cryptography;
using System.Text;

namespace BookingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookingSystemContext _context;
        public AccountController(BookingSystemContext context)
        {
            _context = context;
        }
        
        //Index is where login page is linked to
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            var user = _context.Patients
                .FirstOrDefault(p => p.EmailAddress == username && p.PasswordHash == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View("Index");
            }

            // ✅ Store user in session
            HttpContext.Session.SetString("UserName", user.PatientName);
            HttpContext.Session.SetString("UserEmail", user.EmailAddress);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(
            string fullName,
            string surname,
            string idNumber,
            DateTime dateOfBirth,
            string email,
            string mobileNumber,
            string password,
            string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match");
                return View();
            }
            string hashedPassword = HashPassword(password);
            
            var patient = new Patient
            {
                PatientName = fullName,
                PatientSurname = surname,
                PassportNumber = idNumber,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                PasswordHash = hashedPassword,
                MobileNumber = mobileNumber
            };
            _context.Patients.Add(patient);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        


    }
}
