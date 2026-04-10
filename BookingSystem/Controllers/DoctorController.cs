using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;
using BookingSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

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

            ViewBag.Medications = new List<SelectListItem>
            {
                new SelectListItem { Value = "Paracetamol", Text = "Paracetamol" },
                new SelectListItem { Value = "Ibuprofen", Text = "Ibuprofen" },
                new SelectListItem { Value = "Amoxicillin", Text = "Amoxicillin" },
                new SelectListItem { Value = "Cough Syrup", Text = "Cough Syrup" },
                new SelectListItem { Value = "Vitamin C", Text = "Vitamin C" }
            };

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAppointment(int appointmentId, string symptoms, string description, string medicationName)
        {
            var doctorId = HttpContext.Session.GetInt32("DoctorId");

            if (doctorId == null)
            {
                return RedirectToAction("Index", "Doctor");
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.DoctorId == doctorId.Value);

            if (appointment == null)
            {
                return NotFound();
            }

            var consultationRecord = new ConsultationRecord
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Symptoms = symptoms,
                Description = description,
                MedicationName = medicationName,
                
            };

            _context.ConssultationRecords.Add(consultationRecord);

            // optional: remove appointment after confirmation
            _context.Appointments.Remove(appointment);

            await _context.SaveChangesAsync();

            return RedirectToAction("AppointmentsPage");
        }
    }
}