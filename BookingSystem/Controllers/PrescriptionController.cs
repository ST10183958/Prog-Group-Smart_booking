using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Data;

namespace BookingSystem.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly BookingSystemContext _context;

        public PrescriptionController(BookingSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
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
    }
}