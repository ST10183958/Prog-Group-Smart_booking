using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Data;
using BookingSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BookingSystem.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly BookingSystemContext _context;

        public PrescriptionController(BookingSystemContext context)
        {
            _context = context;
        }

        // Patient: view prescriptions assigned to the signed-in patient
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Patient")
            {
                return RedirectToAction("Index", "Home");
            }

            var patientId = HttpContext.Session.GetInt32("PatientId");
            if (patientId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var prescriptions = await _context.Prescription
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Where(p => p.PatientId == patientId.Value)
                .OrderByDescending(p => p.PrescriptionId)
                .ToListAsync();

            return View(prescriptions);
        }

        // Doctor: open confirm appointment page
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

        // Doctor: save prescription for selected patient
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

            var prescription = new Prescription
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                MedicineName = medicine.MedicineName,
                Quantity = 1,
                Dosage = medicine.Dosage
            };

            _context.Prescription.Add(prescription);

            if (medicine.Stock > 0)
            {
                medicine.Stock -= 1;
            }

            _context.Appointments.Remove(appointment);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Prescription assigned successfully.";
            return RedirectToAction("AppointmentsPage", "Doctor");
        }
    }
}