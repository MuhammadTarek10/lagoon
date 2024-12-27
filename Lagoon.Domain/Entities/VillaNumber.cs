using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lagoon.Domain.Entities
{
    public class VillaNumber
    {
        [Key]
        public int Number { get; set; }

        [ForeignKey("Villa")]
        public Guid VillaId { get; set; }
        [ValidateNever]
        public required Villa Villa { get; set; }

        [StringLength(250)]
        public string? SpecialDetails { get; set; }
    }
}
