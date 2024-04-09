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

        // GET: api/power
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

        // GET: api/power/5
        [HttpGet("{id}")]
        public async Task<ActionResult<powertbl>> Getpower(int id)
        {
            if (_context.powertbl == null)
            {
                return NotFound();
            }
            var power = await _context.powertbl.FindAsync(id);

            if (power == null)
            {
                return NotFound();
            }

            return power;
        }

        // PUT: api/power/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpower(int id, powertbl power)
        {
            if (id != power.id)
            {
                return BadRequest();
            }

            _context.Entry(power).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!powerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/power
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<powertbl>> Postpower(powertbl power)
        {
            if (_context.powertbl == null)
            {
                return Problem("Entity set 'SolarManagementContext.power'  is null.");
            }
            _context.powertbl.Add(power);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpower", new { id = power.id }, power);
        }

        [Route("[controller]/[action]/{volt}/{current}/{power}/{espNum}")]
        public async Task<ActionResult<powertbl>> InsertData(decimal volt, decimal current, decimal power, string espNum)
        {
            powertbl newPOwer = new powertbl
            {
                volt = volt,
                Ampere = current,
                power = power,
                EspNum = espNum,
                datetimecreated = ToUTC8(),
            };

            _context.powertbl.Add(newPOwer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpower", new { id = newPOwer.id }, newPOwer);
        }

        // DELETE: api/power/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletepower(int id)
        {
            if (_context.powertbl == null)
            {
                return NotFound();
            }
            var power = await _context.powertbl.FindAsync(id);
            if (power == null)
            {
                return NotFound();
            }

            _context.powertbl.Remove(power);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool powerExists(int id)
        {
            return (_context.powertbl?.Any(e => e.id == id)).GetValueOrDefault();
        }


        public static DateTime ToUTC8()
        {
            // Get the current date and time
            DateTime localTime = DateTime.Now;

            // Define the target time zone (UTC+8)
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");  // Replace with your desired time zone ID if needed

            // Convert local time to UTC+8 time
            DateTime utcPlus8Time = TimeZoneInfo.ConvertTimeToUtc(localTime, targetTimeZone);

            return utcPlus8Time;
        }
    }
}
