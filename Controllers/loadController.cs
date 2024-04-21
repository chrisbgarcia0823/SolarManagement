using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarManagement.Data;
using SolarManagement.Models;

namespace SolarManagement.Controllers
{
    public class loadController : Controller
    {
        private readonly SolarManagementContext _context;

        public loadController(SolarManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<ActionResult<loadtbl>> GetloadState(int id)
        {
            var loadQuery = from load in _context.loadtbl where load.id == id select load;

            var result = await loadQuery.FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [Route("[controller]/[action]/{id}/{state}")]
        public async Task<ActionResult<loadtbl>> UpdateloadState(int id, int state)
        {
            var loadQuery = from load in _context.loadtbl where load.id == id select load;

            var result = await loadQuery.FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            //update the table
            result.state = state;
            await _context.SaveChangesAsync();

            return result;
        }

    }
}
