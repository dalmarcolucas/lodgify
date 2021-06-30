using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Domain.Rental;

namespace VacationRental.Api.Services
{
    public static class BookingService
    {
        public static bool OverlapBooking(in DateTime start, int nights, Booking booking)
        {
            var conflict = booking.Start <= start.Date && booking.EndWithPreparation > start.Date
                            || booking.Start < start.AddDays(nights) && booking.EndWithPreparation >= start.AddDays(nights)
                            || booking.Start > start && booking.EndWithPreparation < start.AddDays(nights);

            return conflict;
        }

        public static int GetOccupiedUnits(IEnumerable<Booking> bookings, DateTime start, int nights, int idRental, int idBooking = 0)
        {
            var occupiedUnits = 0;
            for (var i = 0; i < nights; i++)
                occupiedUnits = Math.Max(occupiedUnits, bookings.Count(booking => idRental == booking.RentalId
                   && booking.Id != idBooking
                   && OverlapBooking(start, nights, booking)));

            return occupiedUnits;
        }
    }
}
