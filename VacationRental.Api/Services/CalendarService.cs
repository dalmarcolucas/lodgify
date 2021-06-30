using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Api.Services
{
    public class CalendarService
    {
        private readonly IRentalRepository _rentalRepository;

        public CalendarService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        private void Validation(int rentalId, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentalRepository.GetAllRentals().ToDictionary(x => x.Id).ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");
        }

        public CalendarViewModel GetCalendar(int rentalId, DateTime start, int nights)
        {
            Validation(rentalId, nights);

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                };

                foreach (var booking in _rentalRepository.GetAllBookings())
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date
                        && booking.End > date.Date)
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });


                    if (booking.RentalId == rentalId
                        && booking.End <= date.Date
                        && booking.EndWithPreparation > date.Date)
                        date.PreparationTimes.Add(new CalendarPreparationTimesViewModel { Unit = booking.Unit });
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
