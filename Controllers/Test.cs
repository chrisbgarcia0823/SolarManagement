using Microsoft.AspNetCore.Mvc;

namespace SolarManagement.Controllers
{
    public class Test : Controller
    {
        public IActionResult TestIndex()
        {
            return View();
        }

        public IActionResult Test2()
        {
            return View();
        }
    }
}
