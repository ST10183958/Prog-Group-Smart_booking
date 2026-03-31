using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    public class AppointmentDetailsController : Controller
    {
        private readonly BookingSystemContext _context;

        public AppointmentDetailsController(BookingSystemContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {

            var doctors = await _context.Doctors.ToListAsync();
            ViewBag.PatientName = HttpContext.Session.GetInt32("Username");
            return View(doctors);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAppointment(
            string province,
            string suburb,
            DateTime appointmentDate,
            string symptoms,
            int doctorId)
        {
            var patientId = HttpContext.Session.GetInt32("PatientId");

            // prohibit booking if not signed in
            if (patientId == null)
            {
                TempData["BookingError"] = "You must be logged in before booking an appointment.";
                return RedirectToAction("Index", "Account");
            }

            if (string.IsNullOrWhiteSpace(symptoms))
            {
                ModelState.AddModelError("", "Please describe your symptoms.");
            }

            if (doctorId <= 0)
            {
                ModelState.AddModelError("", "Please select a doctor.");
            }

            if (!ModelState.IsValid)
            {
                var doctors = await _context.Doctors.ToListAsync();
                ViewBag.PatientName = HttpContext.Session.GetString("UserName");
                return View("Index", doctors);
            }

            // You can change this logic later if you want real time slots.
            int appointmentSession = 1;

            var appointment = new Appointment
            {
                Province = province,
                Surburb = suburb,
                AppointmentSession = appointmentSession,
                AppointmentIllness = symptoms,
                PreferredAppointmentDate = appointmentDate,
                PatientId = patientId.Value,
                DoctorId = doctorId
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            TempData["BookingSuccess"] = "Appointment booked successfully.";
            return RedirectToAction("Index");
            return View();
        }
    }
}
