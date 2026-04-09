using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Data;

namespace BookingSystem.Controllers;

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
        var Doctor = _context.Doctors
            .FirstOrDefault(D => D.DoctorEmail == DoctorEmail && D.DoctorPassword == DoctorPassword);

        if (Doctor == null)
        {
            ModelState.AddModelError("Email", "Email Not Found");
            return View("Index");
        }
        
        //store user in session
        HttpContext.Session.SetString("Email", Doctor.DoctorEmail);
        
        return RedirectToAction("AppointmentsPage", "Doctor");
    }
    
    public async Task<IActionResult> AppointmentsPage()
    {
        return View();
    }
}