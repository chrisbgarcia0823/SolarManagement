using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarManagement.Models
{
    public class batterytbl
    {
        [Key]
        public int id {  get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? volt { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? Ampere { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? power { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? temp { get; set; }

        public int? batt { get; set; }

        [DataType(DataType.Date)]
        public DateTime? dttmcreated { get; set; }

    }
}
