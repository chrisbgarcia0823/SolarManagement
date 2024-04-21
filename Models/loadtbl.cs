using System.ComponentModel.DataAnnotations;

namespace SolarManagement.Models
{
    public class loadtbl
    {
        [Key]
        public int id {  get; set; }

        public int state { get; set; }
    }
}
