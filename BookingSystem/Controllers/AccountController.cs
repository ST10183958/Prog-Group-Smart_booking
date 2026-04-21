using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace BookingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookingSystemContext _context;

        public AccountController(BookingSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError("", "Email is required.");
                return View("Index");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Password is required.");
                return View("Index");
            }

            string hashedPassword = HashPassword(password);

            var user = _context.Patients
                .FirstOrDefault(p =>
                    p.EmailAddress.ToLower() == username.ToLower() &&
                    p.PasswordHash == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View("Index");
            }

            HttpContext.Session.Clear();

            HttpContext.Session.SetInt32("PatientId", user.PatientId);
            HttpContext.Session.SetString("UserName", user.PatientName);
            HttpContext.Session.SetString("UserEmail", user.EmailAddress);
            HttpContext.Session.SetString("UserRole", "Patient");

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
            if (string.IsNullOrWhiteSpace(fullName))
            {
                ModelState.AddModelError("", "Full name is required.");
                return View();
            }

            if (string.IsNullOrWhiteSpace(surname))
            {
                ModelState.AddModelError("", "Surname is required.");
                return View();
            }

            if (string.IsNullOrWhiteSpace(idNumber))
            {
                ModelState.AddModelError("", "ID/Passport number is required.");
                return View();
            }

            if (dateOfBirth == default)
            {
                ModelState.AddModelError("", "Date of birth is required.");
                return View();
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Email is required.");
                return View();
            }

            if (string.IsNullOrWhiteSpace(mobileNumber))
            {
                ModelState.AddModelError("", "Mobile number is required.");
                return View();
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Password is required.");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View();
            }

            var existingPatient = _context.Patients
                .FirstOrDefault(p => p.EmailAddress.ToLower() == email.ToLower());

            if (existingPatient != null)
            {
                ModelState.AddModelError("", "An account with this email already exists.");
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

            HttpContext.Session.Clear();

            HttpContext.Session.SetInt32("PatientId", patient.PatientId);
            HttpContext.Session.SetString("UserName", patient.PatientName);
            HttpContext.Session.SetString("UserEmail", patient.EmailAddress);
            HttpContext.Session.SetString("UserRole", "Patient");

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

