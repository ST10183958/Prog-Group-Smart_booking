using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class PrescriptionManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
