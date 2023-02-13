using MagicVilla_Web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.VM
{
    public class VillaCreateNumberVM
    {
        public VillaCreateNumberVM()
        {
            VillaNumber = new VillaCreateNumberDTO();
        }
        public VillaCreateNumberDTO VillaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
