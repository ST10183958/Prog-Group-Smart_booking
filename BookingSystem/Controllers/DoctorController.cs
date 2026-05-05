using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using BookingSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookingSystem.Services;
using System.Linq;
using System.Threading.Tasks;

namespace BookingSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly BookingSystemContext _context;
        private readonly EmailService _emailService;

        public DoctorController(BookingSystemContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
                return RedirectToAction("Index", "Doctor");

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId.Value);

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
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("AppointmentsPage");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmAppointment(int appointmentId)
        {
            var doctorId = HttpContext.Session.GetInt32("DoctorId");

            if (doctorId == null)
                return RedirectToAction("Index", "Doctor");

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.DoctorId == doctorId.Value);

            if (appointment == null)
                return NotFound();

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
                return RedirectToAction("Index", "Doctor");

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.DoctorId == doctorId.Value);

            if (appointment == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(symptoms))
                ModelState.AddModelError("", "Symptoms are required.");

            if (string.IsNullOrWhiteSpace(description))
                ModelState.AddModelError("", "Description / Notes are required.");

            var medicine = await _context.Medicines
                .FirstOrDefaultAsync(m => m.MedicineId == medicineId);

            if (medicine == null)
                ModelState.AddModelError("", "Please select a valid medicine.");

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

            // =========================
            // SAFELY CAPTURE DATA FIRST
            // =========================
            var patientEmail = appointment.Patient.EmailAddress;
            var patientName = appointment.Patient.PatientName;
            var doctorName = appointment.Doctor.DoctorName;

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
                medicine.Stock--;

            _context.Appointments.Remove(appointment);

            await _context.SaveChangesAsync();

            // =========================
            // EMAIL NOTIFICATION
            // =========================
            await _emailService.SendEmailAsync(
                patientEmail,
                "Appointment Confirmed",
                $"Hello {patientName},\n\n" +
                $"Your appointment has been confirmed by Dr. {doctorName}.\n\n" +
                $"Symptoms: {symptoms}\n" +
                $"Medication: {medicine.MedicineName}\n\n" +
                $"Please check your portal for details."
            );

            TempData["SuccessMessage"] = "Appointment confirmed and patient notified.";

            return RedirectToAction("AppointmentsPage");
        }
    }
}