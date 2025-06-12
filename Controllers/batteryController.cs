using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarManagement.Data;
using SolarManagement.Models;

namespace SolarManagement.Controllers
{
    public class batteryController : Controller
    {
        private readonly SolarManagementContext _context;

        public batteryController(SolarManagementContext context)
        {
            context.Database.SetCommandTimeout(0);
            _context = context;
        }

        // GET: api/power
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<ActionResult<IEnumerable<batterytbl>>> GetBatteryData()
        {
            if (_context.batterytbl == null)
            {
                return NotFound();
            }
            return await _context.batterytbl.ToListAsync();
        }

        // GET: api/power/5
        [HttpGet]
        [Route("[controller]/[action]/{batteryNum}")]
        public async Task<ActionResult<List<batterytbl>>> GetBatteryData(int batteryNum)
        {
            if (_context.powertbl == null)
            {
                return NotFound();
            }

            var battQuery = from b in _context.batterytbl where b.batt == batteryNum select b;

            var batt = await battQuery.ToListAsync();

            if (batt == null)
            {
                return NotFound();
            }

            return batt;
        }

        [Route("[controller]/[action]/{volt}/{current}/{power}/{temp}/{battNum}")]
        public async Task<ActionResult<batterytbl>> InsertData(decimal volt, decimal current, decimal power, decimal temp, int battNum)
        {
            batterytbl newData = new batterytbl
            {
                //volt = volt,
                volt = ((decimal)0.3402 * volt) + (decimal)1.7876, //calibrated voltage
                Ampere = current,
                power = power,
                //temp = temp,
                temp = ((decimal)1.008 * temp) - (decimal)0.2405, //calibrated temperature
                batt = battNum,
                dttmcreated = ToUTC8(),
            };

            _context.batterytbl.Add(newData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpower", new { id = newData.id }, newData);
        }

        public static DateTime ToUTC8()
        {

            // Get the time zone information for the Philippines (Asia/Manila)
            TimeZoneInfo philippinesZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");

            // Get the current UTC time
            DateTime utcDateTime = DateTime.UtcNow;

            // Convert the UTC time to Philippines time (Asia/Manila)
            DateTime philippinesDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, philippinesZone);

            return philippinesDateTime;

        }
    }
}
