using Lagoon.Domain.Entities;

namespace Lagoon.Application.Utilities
{
    public static class SD
    {
        public const string AdminEndUser = "Admin";
        public const string CustomerEndUser = "Customer";


        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCheckedOut = "CheckedOut";

        public static int VillaRoomsAvailable_Count(Guid villaId,
                                                    IEnumerable<VillaNumber> villaNumbersList,
                                                    DateOnly checkInDate,
                                                    int nights,
                                                    IEnumerable<Booking> bookings)
        {
            List<Guid> bookingInDate = new();
            int finalAvailableRoomForAllNights = int.MaxValue;
            var roomsInVilla = villaNumbersList.Where(x => x.VillaId == villaId).Count();

            for (int i = 0; i < nights; i++)
            {
                var villasBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i)
                && u.CheckOutDate > checkInDate.AddDays(i) && u.VillaId == villaId);

                foreach (var booking in villasBooked)
                {
                    if (!bookingInDate.Contains(booking.Id)) bookingInDate.Add(booking.Id);
                }

                int totalAvailableRooms = roomsInVilla - bookingInDate.Count;

                if (totalAvailableRooms == 0) return 0;
                else finalAvailableRoomForAllNights = Math.Min(finalAvailableRoomForAllNights, totalAvailableRooms);
            }

            return finalAvailableRoomForAllNights;
        }

        // public static RadialBarChartDto GetRadialCartDataModel(int totalCount, double currentMonthCount, double prevMonthCount)
        // {
        //     RadialBarChartDto RadialBarChartDto = new();
        //
        //
        //     int increaseDecreaseRatio = 100;
        //
        //     if (prevMonthCount != 0)
        //     {
        //         increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);
        //     }
        //
        //     RadialBarChartDto.TotalCount = totalCount;
        //     RadialBarChartDto.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
        //     RadialBarChartDto.HasRatioIncreased = currentMonthCount > prevMonthCount;
        //     RadialBarChartDto.Series = new int[] { increaseDecreaseRatio };
        //
        //     return RadialBarChartDto;
        // }
    }
}
