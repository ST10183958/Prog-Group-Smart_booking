using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class AppointmentDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
