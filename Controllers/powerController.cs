using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarManagement.Data;
using SolarManagement.Models;

namespace SolarManagement.Controllers
{
    public class powerController : ControllerBase
    {
        private readonly SolarManagementContext _context;

        public powerController(SolarManagementContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<ActionResult<IEnumerable<powertbl>>> Getpower()
        {
            if (_context.powertbl == null)
            {
                return NotFound();
            }
            return await _context.powertbl.ToListAsync();
        }

        [HttpGet]
        [Route("[controller]/[action]/{espNum}")]
        public async Task<ActionResult<List<powertbl>>> Getpower(string espNum)
        {
            if (_context.powertbl == null)
            {
                return NotFound();
            }
            
            var powerQuery = from p in _context.powertbl where p.EspNum.ToLower() == espNum.ToLower() select p;

            var power = await powerQuery.ToListAsync();

            if (power == null)
            {
                return NotFound();
            }

            return power;
        }



        [Route("[controller]/[action]/{volt}/{current}/{power}/{espNum}/{energy}/{freq}/{pf}")]
        public async Task<ActionResult<powertbl>> InsertData(decimal volt, decimal current, decimal power, string espNum, decimal energy, decimal freq, decimal pf)
        {
            powertbl newPOwer = new powertbl
            {
                volt = volt,
                Ampere = current,
                power = power,
                EspNum = espNum,
                energy = energy,
                freq = freq,
                pf = pf,
                datetimecreated = ToUTC8(),
            };

            _context.powertbl.Add(newPOwer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpower", new { id = newPOwer.id }, newPOwer);
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
