using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarManagement.Models
{
    public class powertbl
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

        [NotMapped]
        public string? LoadType
        {
            get
            {
                if (EspNum.ToLower() == "1")
                {
                    return "Critical Load";
                }
                else if (EspNum.ToLower() == "2")
                {
                    return "Medium Load";
                }
                else
                {
                    return "Normal Load";
                }
            }
            set { }
        }

    }
}
