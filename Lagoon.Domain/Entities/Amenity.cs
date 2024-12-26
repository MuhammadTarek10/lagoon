using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lagoon.Domain.Entities
{
    public class Amenity
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        [ForeignKey("Villa")]
        [Display(Name = "Villa")]
        public Guid VillaId { get; set; }

        [ValidateNever]
        public required Villa Villa { get; set; }
    }
}
