using Microsoft.AspNetCore.Mvc;
using SolarManagement.Models;
using System.Diagnostics;
using SolarManagement.Data;
using Microsoft.EntityFrameworkCore;
using SolarManagement.ViewModel;
using System.Text;

namespace SolarManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly SolarManagementContext _context;

        public HomeController(SolarManagementContext context)
        {
            context.Database.SetCommandTimeout(0);
            _context = context;
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
                                      TimeData = battery.dttmcreated.Value.ToString("yyyy-MM-dd HH:mm"),
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
                                      TimeData = battery.dttmcreated.Value.ToString("yyyy-MM-dd HH:mm"),
                                      DateData = battery.dttmcreated.Value.ToString("MMM-dd-yyyy"),
                                  };

            return await batteryVoltages.FirstOrDefaultAsync();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetData()
        {
            var query = from batt in _context.batterytbl
                        orderby batt.id descending
                        select new ForCSV
                        {
                            Id = batt.id,
                            Voltage = batt.volt,
                            Temperature = batt.temp,
                            BatteryNumber = batt.batt,
                            Date = batt.dttmcreated.Value.ToString("dd-MMM-yyyy HH:mm:ss")
                        };

            var battData = await query.ToListAsync();
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Id,Voltage,Temperature,Battery #,Date");

            foreach (var data in battData)
            {
                sb.AppendLine($"{data.Id},{data.Voltage},{data.Temperature},{data.BatteryNumber},{data.Date}");
            }
            
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"{DateTime.Now.Date}-Battery Data.csv");
        }

        public async Task<IActionResult> GetDataInputCurrent()
        {
            var query = from curr in _context.currenttbl
                        orderby curr.id descending
                        where curr.process == "1"
                        select new ForCSV_Current
                        {
                            Id = curr.id,
                            Current = curr.curr,
                            DateCreated = curr.dttmcreated.Value.ToString("dd-MMM-yyyy HH:mm:ss")
                        };

            var battData = await query.ToListAsync();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Id,Current,Date");

            foreach (var data in battData)
            {
                sb.AppendLine($"{data.Id},{data.Current},{data.DateCreated}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"{DateTime.Now.Date}-Battery Data.csv");
        }
    }
}