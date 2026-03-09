using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
