using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolarManagement.Models;

namespace SolarManagement.Data
{
    public class SolarManagementContext : DbContext
    {
        public SolarManagementContext (DbContextOptions<SolarManagementContext> options)
            : base(options)
        {
        }

        public DbSet<SolarManagement.Models.powertbl> powertbl { get; set; } = default!;
    }
}
