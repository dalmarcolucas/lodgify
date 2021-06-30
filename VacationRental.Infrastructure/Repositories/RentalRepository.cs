using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Rental;

namespace VacationRental.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, Rental> _rentals = new Dictionary<int, Rental>();
        private readonly IDictionary<int, Booking> _bookings = new Dictionary<int, Booking>();

        public RentalRepository()
        {
           
        }

        public IEnumerable<Rental> GetAllRentals()
        {
            return _rentals.Values;
        }
        
        public Rental GetRental(int id)
        {
            if (!_rentals.ContainsKey(id))
                throw new ApplicationException("Rental not found");

            return _rentals[id];
        }

        public Rental AddRental(Rental model)
        {
            var id = _rentals.Keys.Count + 1;

            var rental = new Rental
            {
                Id = id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };

            _rentals.Add(id, rental);

            return rental;
        }

        public Rental UpdateRental(int id, Rental model)
        {
            var rental = GetRental(id);

            rental.PreparationTimeInDays = model.PreparationTimeInDays;
            rental.Units = model.Units;

            return rental;
        }

        public Booking GetBooking(int id)
        {
            if (!_bookings.ContainsKey(id))
                throw new ApplicationException("Booking not found");

            return _bookings[id];
        }

        public Booking AddBooking(Booking model)
        {
            var id = _bookings.Keys.Count + 1;

            var booking = new Booking
            {
                Id = id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = model.Unit,
                PreparationDays = _rentals[model.RentalId].PreparationTimeInDays
            };

            _bookings.Add(id, booking);

            return booking;
        }

        public Booking UpdateBooking(int id, Booking model)
        {
            var booking = GetBooking(id);

            booking.Nights = model.Nights;
            booking.RentalId = model.RentalId;
            booking.Start = model.Start;
            booking.Unit = model.Unit;
            booking.PreparationDays = model.PreparationDays;

            return booking;
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            return _bookings.Values.Select(item =>
            {
                return item.Clone();
            });
        }
    }
}
