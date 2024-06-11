using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SolarManagement.Models
{
    public class powertbl2
    {
        [Key]
        public int id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? volt { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? Ampere { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? power { get; set; }

        public string? EspNum { get; set; }

        [DataType(DataType.Date)]
        public DateTime? datetimecreated { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? energy { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? freq { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? pf { get; set; }
    }
}
