using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarManagement.Models
{
    public class powertbl
    {
        [Key]
        public int id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal volt { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Ampere { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal power { get; set; }

        public string EspNum { get; set; }

        [DataType(DataType.Date)]
        public DateTime datetimecreated { get; set; }
    }
}
