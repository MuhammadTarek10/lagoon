using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lagoon.Domain.Entities
{
    public class Villa
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        [Display(Name = "Price per night")]
        [Range(10, 10000)]
        public decimal Price { get; set; }

        [Display(Name = "Number of rooms")]
        [Range(1, 100)]
        public int Rooms { get; set; }

        [Display(Name = "Square feet")]
        [Range(10, 10000)]
        public int Sqft { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        [Display(Name = "Image Url")]
        [StringLength(250)]
        public string? ImageUrl { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public bool IsAvailable { get; set; } = true;

        [ValidateNever]
        public IEnumerable<Amenity>? Amenities { get; set; }
    }
}
