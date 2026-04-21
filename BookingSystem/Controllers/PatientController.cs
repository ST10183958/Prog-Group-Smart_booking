using Microsoft.AspNetCore.Mvc;
using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookingSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly BookingSystemContext _context;

        public PatientController(BookingSystemContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var patientId = HttpContext.Session.GetInt32("PatientId");
            if (patientId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.PatientId == patientId.Value);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            return View(patient);
        }

        public IActionResult Details()
        {
            var patientId = HttpContext.Session.GetInt32("PatientId");
            if (patientId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.PatientId == patientId.Value);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            return View(patient);
        }

        public IActionResult Edit()
        {
            var patientId = HttpContext.Session.GetInt32("PatientId");
            if (patientId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.PatientId == patientId.Value);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Patient updatedPatient)
        {
            var patientId = HttpContext.Session.GetInt32("PatientId");
            if (patientId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.PatientId == patientId.Value);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            patient.PassportNumber = updatedPatient.PassportNumber;
            patient.DateOfBirth = updatedPatient.DateOfBirth;
            patient.EmailAddress = updatedPatient.EmailAddress;
            patient.MobileNumber = updatedPatient.MobileNumber;

            _context.SaveChanges();

            HttpContext.Session.SetString("UserEmail", patient.EmailAddress);
            HttpContext.Session.SetString("UserName", patient.PatientName);

            TempData["SuccessMessage"] = "Your details have been updated successfully!";

            return RedirectToAction("Details");
        }

        public async Task<IActionResult> Appointments()
        {
            var patientId = HttpContext.Session.GetInt32("PatientId");
            if (patientId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == patientId.Value);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patient.PatientId)
                .ToListAsync();

            return View(appointments);
        }

        public async Task<IActionResult> Prescriptions()
        {
            var patientId = HttpContext.Session.GetInt32("PatientId");
            if (patientId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == patientId.Value);

            if (patient == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var prescriptions = await _context.Prescription
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Where(p => p.PatientId == patient.PatientId)
                .ToListAsync();

            return View(prescriptions);
        }
    }
}