using Microsoft.AspNetCore.Mvc;
using SolarManagement.Models;
using System.Diagnostics;
using SolarManagement.Data;
using Microsoft.EntityFrameworkCore;
using SolarManagement.ViewModel;

namespace SolarManagement.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly SolarManagementContext _context;

        public HomeController(SolarManagementContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("[controller]/[action]/{espNum}")]
        public async Task<ActionResult<IEnumerable<LoadData>>> LoadPowerData(string espNum)
        {
            var query = from data in _context.powertbl
                        where data.EspNum.ToLower() == espNum.ToLower()
                        select new LoadData
                        {
                            TimeData = data.datetimecreated.Value.ToString("HH:mm"),
                            DateData = data.datetimecreated.Value.ToString("MMM-dd-yyyy"),
                            Ampere = data.Ampere,
                            volt = data.volt,
                            power = data.power,
                        };

            return await query.ToListAsync();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}