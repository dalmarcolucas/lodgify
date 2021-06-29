using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public static class RentalService
    {
        internal static void UpdateRental(ref RentalViewModel rental, IDictionary<int, BookingViewModel> bookings, RentalBindingModel model)
        {
            var rentalId = rental.Id;

            var bookingsToUpdate = bookings.Values.Where(x => x.RentalId == rentalId).Select(item =>
            {
                var clone = item.Clone();
                clone.PreparationDays = model.PreparationTimeInDays;
                return clone;
            });

            foreach (BookingViewModel booking in bookingsToUpdate)
            {
                if (BookingService.GetOccupiedUnits(bookingsToUpdate.ToDictionary(x=> x.Id), booking.Start.Date, booking.Nights, booking.RentalId, booking.Id) >= model.Units)
                    throw new ApplicationException("Is not possible update the rental. There is no availability.");
            }

            foreach(var booking in bookings.Values.Where(x => x.RentalId == rentalId))
                booking.PreparationDays = model.PreparationTimeInDays;

            rental.PreparationTimeInDays = model.PreparationTimeInDays;
            rental.Units = model.Units;
        }
    }
}
