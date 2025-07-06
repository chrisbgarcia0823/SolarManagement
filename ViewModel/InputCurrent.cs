using System.ComponentModel.DataAnnotations;

namespace SolarManagement.ViewModel
{
    public class InputCurrent
    {
        [Key]
        public int Id { get; set; }

        public decimal? Current { get; set; }

        public string? TimeData { get; set; }

        public string? DateData { get; set; }
    }
}
