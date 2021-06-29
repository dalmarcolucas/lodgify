using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public static class BookingService
    {
        public static bool OverlapBooking(in DateTime start, int nights, BookingViewModel booking)
        {
            var conflict = booking.Start <= start.Date && booking.EndWithPreparation > start.Date
                            || booking.Start < start.AddDays(nights) && booking.EndWithPreparation >= start.AddDays(nights)
                            || booking.Start > start && booking.EndWithPreparation < start.AddDays(nights);

            return conflict;
        }

        public static int GetOccupiedUnits(IDictionary<int, BookingViewModel> bookings, DateTime start, int nights, int idRental, int idBooking = 0)
        {
            var occupiedUnits = 0;
            for (var i = 0; i < nights; i++)
                occupiedUnits = Math.Max(occupiedUnits, bookings.Count(booking => idRental == booking.Value.RentalId
                   && booking.Value.Id != idBooking
                   && OverlapBooking(start, nights, booking.Value)));

            return occupiedUnits;
        }
    }
}
