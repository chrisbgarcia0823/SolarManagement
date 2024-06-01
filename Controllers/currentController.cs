using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarManagement.Data;
using SolarManagement.Models;

namespace SolarManagement.Controllers
{
    public class currentController : Controller
    {

        private SolarManagementContext _context;

        public currentController(SolarManagementContext context)
        {
            _context = context;
        }

        [Route("[controller]/[action]/{current}/{batt}/")]
        public async Task<ActionResult<currenttbl>> InsertData(decimal current, string batt)
        {
            currenttbl newData = new currenttbl
            {
                curr = current,
                process = batt,
                dttmcreated = ToUTC8(),
            };

            _context.currenttbl.Add(newData);
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
