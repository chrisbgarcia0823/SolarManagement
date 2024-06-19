using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarManagement.ViewModel
{
    [NotMapped]
    public class BatteryVoltages
    {
        [Key]
        public int Id { get; set; }

        public int? batterNumber { get; set; }

        public decimal? voltage { get; set; }

        public decimal? temperature { get; set; }

        public decimal? current { get; set; }

        public string? TimeData { get; set; }

        public string? DateData { get; set; }
    }
}
