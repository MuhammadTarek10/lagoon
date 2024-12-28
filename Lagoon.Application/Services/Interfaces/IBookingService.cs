using Lagoon.Domain.Entities;

namespace Lagoon.Application.Services.Interfaces
{
    public interface IBookingService
    {
        Task CreateBookingAsync(Booking booking);
        Task<Booking?> GetBookingByIdAsync(Guid bookingId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync(string? userId = null, string? statusFilterList = null);

        Task UpdateStatus(Guid bookingId, string bookingStatus, int villaNumber);
        Task UpdateStripePaymentID(Guid bookingId, string sessionId, string paymentIntentId);

        public Task<IEnumerable<int>> GetCheckedInVillaNumbersAsync(Guid villaId);
    }
}
