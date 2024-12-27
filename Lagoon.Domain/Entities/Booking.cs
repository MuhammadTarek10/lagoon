using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lagoon.Domain.Entities
{
    public class Booking
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey("User")]
        public string? UserId { get; set; }
        [ValidateNever]
        public ApplicationUser? User { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        [Required]
        [ForeignKey("Villa")]
        public Guid VillaId { get; set; }
        [ValidateNever]
        public required Villa Villa { get; set; }

        [Display(Name = "Total cost")]
        public decimal TotalCost { get; set; }

        [Display(Name = "Number of nights")]
        public int Nights { get; set; }

        public string? Status { get; set; }

        [Display(Name = "Check-in date")]
        public DateOnly CheckInDate { get; set; }

        [Display(Name = "Check-out date")]
        public DateOnly CheckOutDate { get; set; }

        [Display(Name = "Booking date")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        public bool IsPaymentSuccessful { get; set; } = false;
        public DateOnly PaymentDate { get; set; }

        public string? StripeSessionId { get; set; }
        public string? StripePaymentIntentId { get; set; }

        public DateOnly ActualCheckIn { get; set; }
        public DateOnly ActualCheckOut { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}
