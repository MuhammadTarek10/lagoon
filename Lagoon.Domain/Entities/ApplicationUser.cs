using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Lagoon.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
