using Microsoft.AspNetCore.Mvc;

namespace SolarManagement.Controllers
{
    public class LoadMonitoringController : Controller
    {
        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult CriticalLoad()
        {
            return View();
        }

        public IActionResult MediumLoad()
        {
            return View();
        }

        public IActionResult NormalLoad()
        {
            return View();
        }
    }
}
