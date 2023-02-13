using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Villa2.Model.DTO
{
    public class VillaUpdateNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
    
    }
}
