using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return _bookings[bookingId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            ValidateBooking(model);

            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = BookingService.GetOccupiedUnits(_bookings, model.Start.Date, model.Nights, model.RentalId) + 1,
                PreparationDays = _rentals[model.RentalId].PreparationTimeInDays
            });

            return key;
        }

        private void ValidateBooking(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");

            if (!_rentals.ContainsKey(model.RentalId))
                throw new ApplicationException("Rental not found");

            if (BookingService.GetOccupiedUnits(_bookings, model.Start.Date, model.Nights, model.RentalId) >= _rentals[model.RentalId].Units)
                throw new ApplicationException("Not available");
        }
    }
}
