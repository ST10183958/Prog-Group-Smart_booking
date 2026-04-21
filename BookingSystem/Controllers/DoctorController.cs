using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using BookingSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BookingSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly BookingSystemContext _context;

        public DoctorController(BookingSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DoctorLogin(string DoctorEmail, string DoctorPassword)
        {
            var doctor = _context.Doctors
                .FirstOrDefault(d => d.DoctorEmail == DoctorEmail && d.DoctorPassword == DoctorPassword);

            if (doctor == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View("Index");
            }

            HttpContext.Session.SetInt32("DoctorId", doctor.DoctorId);
            HttpContext.Session.SetString("DoctorName", doctor.DoctorName);
            HttpContext.Session.SetString("DoctorSurname", doctor.DoctorSurname);
            HttpContext.Session.SetString("DoctorEmail", doctor.DoctorEmail);

            return RedirectToAction("AppointmentsPage", "Doctor");
        }

        public async Task<IActionResult> AppointmentsPage()
        {
            var doctorId = HttpContext.Session.GetInt32("DoctorId");

            if (doctorId == null)
            {
                return RedirectToAction("Index", "Doctor");
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId.Value);

            if (doctor == null)
            {
                return RedirectToAction("Index", "Doctor");
            }

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId.Value)
                .OrderBy(a => a.PreferredAppointmentDate)
                .ToListAsync();

            ViewBag.DoctorId = doctor.DoctorId;
            ViewBag.DoctorName = doctor.DoctorName;
            ViewBag.DoctorSurname = doctor.DoctorSurname;

            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineAppointment(int appointmentId)
        {
            var doctorId = HttpContext.Session.GetInt32("DoctorId");

            if (doctorId == null)
            {
                return RedirectToAction("Index", "Doctor");
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.DoctorId == doctorId.Value);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("AppointmentsPage");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmAppointment(int appointmentId)
        {
            var doctorId = HttpContext.Session.GetInt32("DoctorId");

            if (doctorId == null)
            {
                return RedirectToAction("Index", "Doctor");
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.DoctorId == doctorId.Value);

            if (appointment == null)
            {
                return NotFound();
            }

            var medicines = await _context.Medicines
                .OrderBy(m => m.MedicineName)
                .ToListAsync();

            ViewBag.Medications = medicines.Select(m => new SelectListItem
            {
                Value = m.MedicineId.ToString(),
                Text = $"{m.MedicineName} (Stock: {m.Stock}, Dosage: {m.Dosage})"
            }).ToList();

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAppointment(
            int appointmentId,
            string symptoms,
            string description,
            int medicineId)
        {
            var doctorId = HttpContext.Session.GetInt32("DoctorId");

            if (doctorId == null)
            {
                return RedirectToAction("Index", "Doctor");
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.DoctorId == doctorId.Value);

            if (appointment == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(symptoms))
            {
                ModelState.AddModelError("", "Symptoms are required.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                ModelState.AddModelError("", "Description / Notes are required.");
            }

            var medicine = await _context.Medicines
                .FirstOrDefaultAsync(m => m.MedicineId == medicineId);

            if (medicine == null)
            {
                ModelState.AddModelError("", "Please select a valid medicine.");
            }

            if (!ModelState.IsValid)
            {
                var medicines = await _context.Medicines
                    .OrderBy(m => m.MedicineName)
                    .ToListAsync();

                ViewBag.Medications = medicines.Select(m => new SelectListItem
                {
                    Value = m.MedicineId.ToString(),
                    Text = $"{m.MedicineName} (Stock: {m.Stock}, Dosage: {m.Dosage})"
                }).ToList();

                return View(appointment);
            }

            var consultationRecord = new ConsultationRecord
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Symptoms = symptoms,
                Description = description,
                MedicationName = medicine.MedicineName
            };

            _context.ConssultationRecords.Add(consultationRecord);

            if (medicine.Stock > 0)
            {
                medicine.Stock -= 1;
            }

            _context.Appointments.Remove(appointment);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Appointment confirmed successfully.";
            return RedirectToAction("AppointmentsPage");
        }
    }
}