using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SolarManagement.ViewModel
{
    public class ForCSV_Current
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? Current { get; set; }

        public string? DateCreated { get; set; }
    }
}
