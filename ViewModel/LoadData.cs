using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SolarManagement.ViewModel
{
    [NotMapped]
    public class LoadData
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

        public string? TimeData { get; set; }

        public string? DateData { get; set; }


    }
}
