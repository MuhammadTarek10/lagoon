using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lagoon.Web.ViewModels
{
    public class RegisterVM
    {

        [Required]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }

        public string? RedirectUrl { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? RoleList { get; set; }

        [Required]
        public string? Role { get; set; }

    }
}
