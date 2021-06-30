using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Api.Services
{
    public class RentalService
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public void ValidateBooking(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");

            var rental = _rentalRepository.GetRental(model.RentalId);
               
            if (BookingService.GetOccupiedUnits(this._rentalRepository.GetAllBookings(), model.Start.Date, model.Nights, model.RentalId) >= rental.Units)
                throw new ApplicationException("Not available");
        }

        internal void UpdateRental(int idRental, RentalBindingModel model)
        {
            var bookingsToUpdate = _rentalRepository.GetAllBookings().Where(x => x.RentalId == idRental).Select(item =>
            {
                item.PreparationDays = model.PreparationTimeInDays;
                return item;
            });

            foreach (var booking in bookingsToUpdate)
            {
                if (BookingService.GetOccupiedUnits(bookingsToUpdate, booking.Start.Date, booking.Nights, booking.RentalId, booking.Id) >= model.Units)
                    throw new ApplicationException("Is not possible update the rental. There is no availability.");
            }

            foreach (var booking in _rentalRepository.GetAllBookings().Where(x => x.RentalId == idRental))
            {
                booking.PreparationDays = model.PreparationTimeInDays;
                _rentalRepository.UpdateBooking(booking.Id, booking);
            }

            _rentalRepository.UpdateRental(idRental, model.ToRental());
        }
    }
}
