using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lagoon.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> FinalizeBooking(Guid id, DateOnly checkInDate, int nights)
        {
            Booking booking = new()
            {
                VillaId = id,
                Villa = (await _unitOfWork.Villa.GetAsync(u => u.Id == id))!,
                CheckInDate = checkInDate,
                Nights = nights
            };

            return View(booking);
        }

    }
}
