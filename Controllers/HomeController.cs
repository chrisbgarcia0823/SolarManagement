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

        public IActionResult BatteryMonitoring()
        {
            return View();
        }

        public async Task<IActionResult> TableView()
        {
            var data = await(from batt in _context.batterytbl orderby batt.id descending select batt).ToListAsync();
            return View(data);
        }

        [HttpGet]
        [Route("[controller]/[action]/{batterNumber}/{parameter}")]
        public IActionResult LiveData(int batterNumber, string parameter)
        {
            TempData["BatteryNumber"] = batterNumber;
            TempData["Parameter"] = parameter;
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

        //FETCH TO INDIVIDUALLY GET AN UPDATE ON THE DATA
        [HttpGet]
        [Route("[controller]/[action]/{espNum}/{update}")]
        public async Task<ActionResult<LoadData>> LoadNewPowerData(string espNum, string update)
        {
            var query = from data in _context.powertbl
                        where data.EspNum.ToLower() == espNum.ToLower()
                        orderby data.id descending
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

        [Route("[controller]/[action]/{batteryNum}")]
        public async Task<ActionResult<List<BatteryVoltages>>> GetBatteryData(int batteryNum)
        {
            var batteryVoltages = from battery in _context.batterytbl where battery.batt == batteryNum
                                  orderby battery.id descending
                                  select new BatteryVoltages
                                  {
                                      Id = battery.id,
                                      voltage = battery.volt,
                                      temperature = battery.temp,
                                      TimeData = battery.dttmcreated.Value.ToString("HH:mm"),
                                      DateData = battery.dttmcreated.Value.ToString("MMM-dd-yyyy"),
                                  };

            return await batteryVoltages.Take(60).OrderBy(data => data.Id).ToListAsync();
        }

        //FETCH TO INDIVIDUALLY GET AN UPDATE ON THE DATA
        [Route("[controller]/[action]/{batteryNum}/{update}")]
        public async Task<ActionResult<BatteryVoltages>> LoadNewBatteryData(int batteryNum, string update)
        {
            var batteryVoltages = from battery in _context.batterytbl
                                  where battery.batt == batteryNum
                                  orderby battery.id descending
                                  select new BatteryVoltages
                                  {
                                      voltage = battery.volt,
                                      temperature = battery.temp,
                                      TimeData = battery.dttmcreated.Value.ToString("HH:mm"),
                                      DateData = battery.dttmcreated.Value.ToString("MMM-dd-yyyy"),
                                  };

            return await batteryVoltages.FirstOrDefaultAsync();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}