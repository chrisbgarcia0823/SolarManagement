using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarManagement.Data;

namespace SolarManagement.Controllers
{
    public class LoadMonitoringController : Controller
    {
        private readonly SolarManagementContext _context;

        public LoadMonitoringController(SolarManagementContext context)
        {
            _context = context;
        }

        public IActionResult Summary()
        {
            return View();
        }

        public async Task<IActionResult> TableView()
        {
            var query = from p in _context.powertbl select p;

            var queryList = await query.ToListAsync();

            return View(queryList);
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
