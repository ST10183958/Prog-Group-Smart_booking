using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
