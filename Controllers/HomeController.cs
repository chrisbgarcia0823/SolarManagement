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
        private readonly SolarManagementContext _context;

        public HomeController(SolarManagementContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OutputEsp1()
        {
            return View();
        }

        public IActionResult OutputEsp2()
        {
            return View();
        }

        public IActionResult OutputEsp3()
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
                            energy = data.energy,
                            freq = data.freq,
                            pf = data.pf,
                        };

            return await query.Take(60).ToListAsync();
        }

        [HttpGet]
        [Route("[controller]/[action]/{espNum}/{update}")]
        public async Task<ActionResult<LoadData>> LoadNewPowerData(string espNum, string update)
        {
            var query = from data in _context.powertbl
                        where data.EspNum.ToLower() == espNum.ToLower()
                        orderby data.id ascending
                        select new LoadData
                        {
                            TimeData = data.datetimecreated.Value.ToString("HH:mm"),
                            DateData = data.datetimecreated.Value.ToString("MMM-dd-yyyy"),
                            Ampere = data.Ampere,
                            volt = data.volt,
                            power = data.power,
                            energy = data.energy,
                            freq = data.freq,
                            pf = data.pf,
                        };

            return await query.FirstOrDefaultAsync();
        }

        [Route("[controller]/[action]")]
        public async Task<ActionResult<List<BatteryVoltages>>> GetBatteryData()
        {
            string sqlQuery = "SELECT * FROM [db3861].[dbo].[vBatteries]";
            var data = _context.batterytbl.FromSqlRaw(sqlQuery);
            var batteryVoltages = from battery in data
                                  select new BatteryVoltages
                                  {
                                      Id = battery.id,
                                      batterNumber = battery.batt,
                                      voltage = battery.volt,
                                      temperature = battery.temp,
                                      TimeData = battery.dttmcreated.Value.ToString("HH:mm"),
                                      DateData = battery.dttmcreated.Value.ToString("MMM-dd-yyyy"),
                                  };

            return await batteryVoltages.ToListAsync();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}