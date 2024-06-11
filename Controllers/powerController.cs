using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarManagement.Data;
using SolarManagement.Models;
using SolarManagement.ViewModel;

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

            var powerQuery = from power in _context.powertbl select power;


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
            
            var powerQuery = from p in _context.powertbl where p.EspNum.ToLower() == espNum.ToLower() orderby p.id descending select p;

            var power = await powerQuery.Take(60).ToListAsync();

            if (power == null)
            {
                return NotFound();
            }

            return power;
        }

        //TO GET THE LOAD DATA
        [Route("[controller]/[action]")]
        public async Task<ActionResult<List<LoadData>>> GetLoadData()
        {
            string sqlQuery = "SELECT * FROM [db3861].[dbo].[vPower]";
            var data = _context.powertbl.FromSqlRaw(sqlQuery);
            var loadData = from power in data
                                  select new LoadData
                                  {
                                      id = power.id,
                                      volt = power.volt,
                                      power = power.power,
                                      Ampere = power.Ampere,
                                      EspNum = power.EspNum,
                                      TimeData = power.datetimecreated.Value.ToString("HH:mm"),
                                      DateData = power.datetimecreated.Value.ToString("MMM-dd-yyyy"),
                                  };

            return await loadData.ToListAsync();
        }

        //FOR INSERT DATA TO POWERTBL 
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

        //FOR INSERT DATA TO POWERTBL2
        [Route("[controller]/[action]/{volt}/{current}/{power}/{espNum}/{energy}/{freq}/{pf}")]
        public async Task<ActionResult<powertbl2>> InsertData2(decimal volt, decimal current, decimal power, string espNum, decimal energy, decimal freq, decimal pf)
        {
            powertbl2 newPOwer = new powertbl2
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

            _context.powertbl2.Add(newPOwer);
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
