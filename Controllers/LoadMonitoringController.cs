using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarManagement.Data;
using SolarManagement.ViewModel;
using System;

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

        public async Task<IActionResult> CriticalLoad()
        {

            var query = from p in _context.powertbl where p.EspNum.ToLower() == "1" select p;

            var queryList = await query.ToListAsync();

            return View(queryList);
        }

        public async Task<IActionResult> MediumLoad()
        {
            var query = from p in _context.powertbl where p.EspNum.ToLower() == "2" select p;

            var queryList = await query.ToListAsync();

            return View(queryList);
        }

        public async Task<IActionResult> NormalLoad()
        {
            var query = from p in _context.powertbl where p.EspNum.ToLower() == "3" select p;

            var queryList = await query.ToListAsync();

            return View(queryList);
        }

        [Route("[controller]/[action]/{espNum}")]
        public async Task<ActionResult<List<LoadData>>> GetLoadData(string espNum)
        {
            string category = "";
            if (espNum == "1")
            {
                category = "Critical Load";
            }
            else if (espNum == "2")
            {
                category = "Normal Load";
            }
            else if (espNum == "3")
            {
                category = "Less Priority Load";
            }
            else
            {
                category = "";
            }

            var loadData = from load in _context.powertbl
                                  where load.EspNum.ToLower() == espNum.ToLower()
                                  orderby load.id descending
                                  select new LoadData
                                  {
                                      id = load.id,
                                      volt = load.volt,
                                      Ampere = load.Ampere,
                                      power = load.power,
                                      TimeData = load.datetimecreated.Value.ToString("HH:mm"),
                                      DateData = load.datetimecreated.Value.ToString("MMM-dd-yyyy"),
                                      EspNum = espNum,
                                      Category = category,
                                  };

            return await loadData.Take(60).OrderBy(data => data.id).ToListAsync();
        }

        //FETCH TO INDIVIDUALLY GET AN UPDATE ON THE DATA
        [Route("[controller]/[action]/{espNum}/{update}")]
        public async Task<ActionResult<LoadData>> UpdateLoadData(string espNum, string update)
        {
            string category = "";
            if (espNum == "1")
            {
                category = "Critical Load";
            }
            else if (espNum == "2")
            {
                category = "Normal Load";
            }
            else if (espNum == "3")
            {
                category = "Less Priority Load";
            }
            else
            {
                category = "";
            }

            var loadData = from load in _context.powertbl
                                  where load.EspNum.ToLower() == espNum.ToLower()
                                  orderby load.id descending
                                  select new LoadData
                                  {
                                      id = load.id,
                                      volt = load.volt,
                                      Ampere = load.Ampere,
                                      power = load.power,
                                      TimeData = load.datetimecreated.Value.ToString("HH:mm"),
                                      DateData = load.datetimecreated.Value.ToString("MMM-dd-yyyy"),
                                      EspNum = espNum,
                                      Category = category,
                                  };

            return await loadData.FirstOrDefaultAsync();
        }

        [Route("[controller]/[action]/{espNum}")]
        public async Task<int> GetLoadState(int espNum)
        {
            var query = await (from load in _context.loadtbl where load.id == espNum select load).FirstOrDefaultAsync();
            if(query == null)
            {
                return 3;
            }

            return query.state;
        }

        [Route("[controller]/[action]/{espNum}/{state}")]
        public async Task<string> UpdateLoadStatus(int espNum, int state)
        {
            var query = await (from load in _context.loadtbl where load.id == espNum select load).FirstOrDefaultAsync();
            if (query == null)
            {
                return "updateError";
            }

            if(state == 1)
            {
                query.state = 0;
                await _context.SaveChangesAsync();
            }
            else
            {
                query.state = 1;
                await _context.SaveChangesAsync();
            }

            return "updateSuccess";
        }

    }
}
