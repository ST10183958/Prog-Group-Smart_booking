using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class PatientListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
