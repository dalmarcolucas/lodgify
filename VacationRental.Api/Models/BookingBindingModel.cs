using System;
using VacationRental.Domain.Rental;

namespace VacationRental.Api.Models
{
    public class BookingBindingModel
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public int Nights { get; set; }

        internal Booking ToBooking()
        {
            return new Booking
            {
                RentalId = this.RentalId,
                Start = this.Start,
                Nights = this.Nights,
            };
        }
    }
}
