using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarManagement.Models
{
    public class currenttbl
    {
        [Key]
        public int id {  get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? curr { get; set; }

        public string? process {  get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? volt { get; set; }

        [DataType(DataType.Date)]
        public DateTime? dttmcreated { get; set; }
    }
}
