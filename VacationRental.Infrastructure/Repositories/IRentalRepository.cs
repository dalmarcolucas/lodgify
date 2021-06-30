using System.Collections.Generic;
using VacationRental.Domain.Rental;

namespace VacationRental.Infrastructure.Repositories
{
    public interface IRentalRepository
    {
        IEnumerable<Rental> GetAllRentals();

        Rental GetRental(int id);

        Rental AddRental(Rental model);

        Rental UpdateRental(int id, Rental model);

        Booking GetBooking(int id);

        Booking AddBooking(Booking model);

        Booking UpdateBooking(int id, Booking model);

        IEnumerable<Booking> GetAllBookings();
    }
}
