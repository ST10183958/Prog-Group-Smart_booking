using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BookingSystem.Controllers.Api
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentApiController : ControllerBase
    {
        private readonly BookingSystemContext _context;

        public AppointmentApiController(BookingSystemContext context)
        {
            _context = context;
        }

        // GET: api/appointments/doctor/2
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.AppointmentIllness,
                    a.PreferredAppointmentDate,
                    Patient = a.Patient.PatientName + " " + a.Patient.PatientSurname,
                    a.Province,
                    a.Surburb
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // GET: api/appointments/patient/4
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientAppointments(int patientId)
        {
            var appointments = await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToListAsync();

            return Ok(appointments);
        }
    }
}