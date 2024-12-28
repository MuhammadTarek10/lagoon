using System.Security.Claims;
using Lagoon.Application.Services.Interfaces;
using Lagoon.Application.Utilities;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Lagoon.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVillaService _villaService;
        private readonly IPaymentService _paymentService;

        public BookingController(IBookingService bookingService,
                                 UserManager<ApplicationUser> userManager,
                                 IVillaService villaService,
                                 IPaymentService paymentService)
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _villaService = villaService;
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> FinalizeBooking(Guid id, DateOnly checkInDate, int nights)
        {
            ClaimsIdentity? claimIdentity = User.Identity as ClaimsIdentity;

            if (claimIdentity == null) return NotFound();

            string? userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return NotFound();

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            Villa? villa = await _villaService.GetVillaByIdAsync(id);

            if (villa == null) return NotFound();

            Booking booking = new()
            {
                VillaId = id,
                Villa = villa,
                UserId = userId,
                Name = user.Name,
                Email = user.Email,
                Phone = user.PhoneNumber,
                CheckInDate = checkInDate,
                Nights = nights,
                TotalCost = villa.Price * nights

            };

            return View(booking);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> FinalizeBooking(Booking booking)
        {
            Villa? villa = await _villaService.GetVillaByIdAsync(booking.VillaId);

            if (villa == null) return NotFound();

            booking.TotalCost = villa.Price * booking.Nights;
            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;

            if (!(await _villaService.IsVillaAvailableByDateAsync(villa.Id, booking.Nights, booking.CheckInDate)))
            {

                return RedirectToAction(nameof(FinalizeBooking),
                    new { id = villa.Id, checkInDate = booking.CheckInDate, nights = booking.Nights });

            }

            await _bookingService.CreateBookingAsync(booking);


            var domain = Request.Scheme + "://" + Request.Host.Value + "/";

            var options = _paymentService.CreateStripeSessionOptions(booking, villa, domain);

            var session = _paymentService.CreateStripeSession(options);

            await _bookingService.UpdateStripePaymentID(booking.Id, session.Id, session.PaymentIntentId);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> BookingConfirmation(Guid bookingId)
        {
            Booking? booking = await _bookingService.GetBookingByIdAsync(bookingId);

            if (booking == null) return NotFound();

            if (booking.Status == SD.StatusPending)
            {
                // WARN:

                // this is a pending order, we need to confirm if payment was successful
                var service = new SessionService();
                Session session = service.Get(booking.StripeSessionId);

                if (session.PaymentStatus == "paid")
                {
                    await _bookingService.UpdateStatus(booking.Id, SD.StatusApproved, 0);
                    await _bookingService.UpdateStripePaymentID(booking.Id, session.Id, session.PaymentIntentId);

                    // _emailService.SendEmailAsync(bookingFromDb.Email, "Booking Confirmation - White Lagoon", "<p>Your booking has been confirmed. Booking ID - " + bookingFromDb.Id + "</p>");
                }
            }

            return View(booking);
        }
    }
}
