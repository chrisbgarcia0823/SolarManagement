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

        // GET: api/power
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
    }
}
