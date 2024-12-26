using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lagoon.Web.ViewModels
{
    public class VillaNumberVM
    {
        public VillaNumber? Number { get; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? VillaList { get; }
    }
}
