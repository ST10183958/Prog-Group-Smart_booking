using Microsoft.AspNetCore.Mvc;
using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly BookingSystemContext _context;

        public PatientController(BookingSystemContext context)
        {
            _context = context;
        }

        // GET: Patient/Dashboard
        public IActionResult Dashboard()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.EmailAddress == userEmail);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            return View(patient);
        }

        // GET: Patient/Details
        public IActionResult Details()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.EmailAddress == userEmail);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            return View(patient);
        }

        // GET: Patient/Edit
        public IActionResult Edit()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.EmailAddress == userEmail);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            return View(patient);
        }

        // POST: Patient/Edit
        // POST: Patient/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Patient updatedPatient)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var patient = _context.Patients
                .FirstOrDefault(p => p.EmailAddress == userEmail);

            if (patient != null)
            {
                // Update all fields regardless
                patient.PatientName = updatedPatient.PatientName;
                patient.PatientSurname = updatedPatient.PatientSurname;
                patient.PassportNumber = updatedPatient.PassportNumber;
                patient.DateOfBirth = updatedPatient.DateOfBirth;
                patient.EmailAddress = updatedPatient.EmailAddress;
                patient.MobileNumber = updatedPatient.MobileNumber;

                _context.SaveChanges();

                // Update session info if email changed
                HttpContext.Session.SetString("UserEmail", patient.EmailAddress);
                HttpContext.Session.SetString("UserName", patient.PatientName);

                // Only show success message if we're coming from the Edit POST
                TempData["SuccessMessage"] = "Your details have been updated successfully!";
            }

            return RedirectToAction("Details");
        }

        // GET: Patient/Appointments
        public async Task<IActionResult> Appointments()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.EmailAddress == userEmail);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            // Get appointments for this patient
            var appointments = await _context.Appointments
                .Where(a => a.PatientName == patient.PatientName + " " + patient.PatientSurname)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Patient/Prescriptions
        public async Task<IActionResult> Prescriptions()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.EmailAddress == userEmail);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            // Get prescriptions for this patient
            var prescriptions = await _context.Prescriptions
                .Where(p => p.PatientName == patient.PatientName && p.PatientSurname == patient.PatientSurname)
                .ToListAsync();

            return View(prescriptions);
        }
    }
}