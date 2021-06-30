using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IRentalRepository _rentalRepository;

        private RentalService _rentalService;

        private RentalService RentalService
        {
            get { return _rentalService ?? (_rentalService = new RentalService(_rentalRepository)); }
        }

        public BookingsController(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            return new BookingViewModel(_rentalRepository.GetBooking(bookingId));
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            RentalService.ValidateBooking(model);
            var booking = model.ToBooking();

            booking.Unit = BookingService.GetOccupiedUnits(_rentalRepository.GetAllBookings(), model.Start.Date, model.Nights, model.RentalId) + 1;

            return new ResourceIdViewModel
            {
                Id = _rentalRepository.AddBooking(booking).Id
            };
        }

    }
}
