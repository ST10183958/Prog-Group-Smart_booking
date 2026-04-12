using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystem.Data;
using BookingSystem.Models;

namespace BookingSystem.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly BookingSystemContext _context;

        public StatisticsController(BookingSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int totalPatients = await _context.Patients.CountAsync();
            int totalDoctors = await _context.Doctors.CountAsync();
            int totalAppointments = await _context.Appointments.CountAsync();
            int totalAdmins = await _context.Admins.CountAsync();

            var statistic = await _context.Statistics
                .OrderByDescending(s => s.StatisticsId)
                .FirstOrDefaultAsync();

            if (statistic == null)
            {
                statistic = new Statistic();
                _context.Statistics.Add(statistic);
            }

            statistic.NumOfPatients = totalPatients;
            statistic.CurrentNumberOfAppointments = totalAppointments;
            statistic.TotalDoctors = totalDoctors;
            statistic.TotalPatients = totalPatients;
            statistic.TotalAdmins = totalAdmins;

            await _context.SaveChangesAsync();

            return View(statistic);
        }
    }
}