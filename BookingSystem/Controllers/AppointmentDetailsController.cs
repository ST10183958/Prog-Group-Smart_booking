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
            return View(doctors);
        }
    }
}
