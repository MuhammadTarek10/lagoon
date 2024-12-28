using Lagoon.Application.Common.Interfaces;
using Lagoon.Application.Services.Interfaces;
using Lagoon.Application.Utilities;
using Lagoon.Domain.Entities;

namespace Lagoon.Application.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task CreateBookingAsync(Booking booking)
        {
            await _unitOfWork.Booking.AddAsync(booking);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync(string? userId = null, string? statusFilterList = null)
        {
            if (statusFilterList is null && userId is null) return await _unitOfWork.Booking.GetAllAsync(includeProperties: "User, Villa");

            if (statusFilterList is null && userId is not null) return await _unitOfWork.Booking.GetAllAsync(u => u.UserId == userId, includeProperties: "User, Villa");

            IEnumerable<string> statusList = statusFilterList!.ToLower().Split(",");

            if (userId is null) return await _unitOfWork.Booking.GetAllAsync(u => statusList.Contains(u.Status.ToLower()), includeProperties: "User, Villa");

            return await _unitOfWork.Booking.GetAllAsync(u => statusList.Contains(u.Status.ToLower()) && u.UserId == userId, includeProperties: "User, Villa");
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid bookingId) => await _unitOfWork.Booking.GetAsync(u => u.Id == bookingId, includeProperties: "User, Villa");

        public async Task<IEnumerable<int>> GetCheckedInVillaNumbersAsync(Guid villaId)
        {
            return (await _unitOfWork.Booking.GetAllAsync(u => u.VillaId == villaId
                                                               && u.Status == SD.StatusCheckedIn)).Select(u => u.VillaNumber);
        }

        public async Task UpdateStatus(Guid bookingId, string bookingStatus, int villaNumber)
        {

            Booking? booking = await _unitOfWork.Booking.GetAsync(m => m.Id == bookingId, tracked: true);

            if (booking is null) return;

            booking.Status = bookingStatus;

            if (bookingStatus == SD.StatusCheckedIn)
            {
                booking.VillaNumber = villaNumber;
                booking.ActualCheckInDate = DateTime.Now;
            }

            if (bookingStatus == SD.StatusCompleted) booking.ActualCheckOutDate = DateTime.Now;

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateStripePaymentID(Guid bookingId, string sessionId, string paymentIntentId)
        {

            Booking? booking = await _unitOfWork.Booking.GetAsync(m => m.Id == bookingId, tracked: true);

            if (booking is null || string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(paymentIntentId)) return;

            booking.StripeSessionId = sessionId;
            booking.StripePaymentIntentId = paymentIntentId;
            booking.PaymentDate = DateTime.Now;
            booking.IsPaymentSuccessful = true;

            await _unitOfWork.SaveAsync();
        }
    }
}
