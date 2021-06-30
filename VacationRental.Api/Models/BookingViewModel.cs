using System;
using VacationRental.Domain.Rental;

namespace VacationRental.Api.Models
{
    public class BookingViewModel : ICloneable
    {
        public BookingViewModel()
        {

        }

        public BookingViewModel(Booking booking)
        {
            Id = booking.Id;
            RentalId = booking.RentalId;
            Start = booking.Start;
            Nights = booking.Nights;
            Unit = booking.Unit;
            PreparationDays = booking.PreparationDays;
        }

        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int Unit { get; set; }
        public int PreparationDays { get; set; }
        public DateTime End { get { return Start.AddDays(Nights); } }
        public DateTime EndWithPreparation { get { return Start.AddDays(Nights + PreparationDays); } }

        public BookingViewModel Clone()
        {
            return new BookingViewModel
            {
                Id = this.Id,
                RentalId = this.RentalId,
                Start = this.Start,
                Nights = this.Nights,
                Unit = this.Unit,
                PreparationDays = this.PreparationDays,
            };
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        internal Booking ToBooking()
        {
            return new Booking()
            {
                Id = this.Id,
                RentalId = this.RentalId,
                Start = this.Start,
                Nights = this.Nights,
                Unit = this.Unit,
                PreparationDays = this.PreparationDays,
            };
        }
    }
}
