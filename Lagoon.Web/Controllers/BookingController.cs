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
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVillaService _villaService;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IPaymentService _paymentService;

        public BookingController(IBookingService bookingService,
                                 UserManager<ApplicationUser> userManager,
                                 IVillaService villaService,
                                 IVillaNumberService villaNumberService,
                                 IPaymentService paymentService)
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _villaService = villaService;
            _villaNumberService = villaNumberService;
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region

        [HttpGet]
        public async Task<IActionResult> GetAll(string? status = null)
        {
            IEnumerable<Booking> bookings;

            if (User.IsInRole(SD.AdminEndUser)) bookings = await _bookingService.GetAllBookingsAsync();

            else
            {
                string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId is null) return NotFound();

                Console.WriteLine($"UserId: {userId}");

                bookings = await _bookingService.GetAllBookingsAsync(userId, status);
            }


            return Json(new { data = bookings });
        }

        #endregion

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
                CheckOutDate = checkInDate.AddDays(nights),
                Nights = nights,
                TotalCost = villa.Price * nights

            };

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizeBooking(Booking booking)
        {
            Villa? villa = await _villaService.GetVillaByIdAsync(booking.VillaId);

            if (villa == null) return NotFound();

            booking.Id = Guid.NewGuid();
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

        public async Task<IActionResult> BookingConfirmation(Guid id)
        {
            Booking? booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null) return NotFound();

            if (booking.Status == SD.StatusPending)
            {
                // this is a pending order, we need to confirm if payment was successful
                var service = new SessionService();
                Session session = service.Get(booking.StripeSessionId);

                if (session.PaymentStatus == "paid")
                {
                    await _bookingService.UpdateStatus(booking.Id, SD.StatusApproved, 0);
                    await _bookingService.UpdateStripePaymentID(booking.Id, session.Id, session.PaymentIntentId);
                    // _emailService.SendEmailAsync(booking.Email, "Booking Confirmation - Lagoon", "<p>Your booking has been confirmed. Booking ID - " + booking.Id + "</p>");
                }
            }

            return View(booking.Id);
        }


        public async Task<IActionResult> BookingDetails(Guid id)
        {
            Booking? booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking is null) return NotFound();

            if (booking.VillaNumber == 0 && booking.Status == SD.StatusApproved)
            {
                List<int> availableVillaNumber = await AssignAvailableVillaNumberByVilla(booking.VillaId);

                booking.VillaNumbers = (await _villaNumberService.GetAllVillaNumbersAsync()).Where(u => u.VillaId == booking.VillaId
                && availableVillaNumber.Any(x => x == u.Number));
            }

            return View(booking);
        }

        [HttpPost]
        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> CheckIn(Booking booking)
        {
            await _bookingService.UpdateStatus(booking.Id, SD.StatusCheckedIn, booking.VillaNumber);
            TempData["Success"] = "Booking Updated Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { id = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> CheckOut(Booking booking)
        {
            await _bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, booking.VillaNumber);
            TempData["Success"] = "Booking Completed Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { id = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> CancelBooking(Booking booking)
        {
            await _bookingService.UpdateStatus(booking.Id, SD.StatusCancelled, 0);
            TempData["Success"] = "Booking Cancelled Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { id = booking.Id });
        }

        private async Task<List<int>> AssignAvailableVillaNumberByVilla(Guid id)
        {
            List<int> availableVillaNumbers = new();

            IEnumerable<VillaNumber> villaNumbers = (await _villaNumberService.GetAllVillaNumbersAsync()).Where(u => u.VillaId == id);

            IEnumerable<int> checkedInVilla = await _bookingService.GetCheckedInVillaNumbersAsync(id);

            foreach (var villaNumber in villaNumbers)
            {
                if (!checkedInVilla.Contains(villaNumber.Number)) availableVillaNumbers.Add(villaNumber.Number);
            }
            return availableVillaNumbers;
        }
    }
}
