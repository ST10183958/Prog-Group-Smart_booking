using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BookingSystem.Controllers.Api
{
    [Route("api/prescriptions")]
    [ApiController]
    public class PrescriptionApiController : ControllerBase
    {
        private readonly BookingSystemContext _context;

        public PrescriptionApiController(BookingSystemContext context)
        {
            _context = context;
        }

        // GET: api/prescriptions/patient/5
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientPrescriptions(int patientId)
        {
            var prescriptions = await _context.Prescription
                .Include(p => p.Doctor)
                .Include(p => p.Patient)
                .Where(p => p.PatientId == patientId)
                .Select(p => new
                {
                    p.PrescriptionId,
                    Patient = p.Patient.PatientName + " " + p.Patient.PatientSurname,
                    Doctor = p.Doctor.DoctorName + " " + p.Doctor.DoctorSurname,
                    p.MedicineName,
                    p.Quantity,
                    p.Dosage
                })
                .ToListAsync();

            return Ok(prescriptions);
        }

        // GET: api/prescriptions/doctor/3
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorPrescriptions(int doctorId)
        {
            var prescriptions = await _context.Prescription
                .Where(p => p.DoctorId == doctorId)
                .ToListAsync();

            return Ok(prescriptions);
        }
    }
}