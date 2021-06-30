using System;

namespace VacationRental.Domain.Rental
{
    public class Booking : ICloneable
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int Unit { get; set; }
        public int PreparationDays { get; set; }
        public DateTime End { get { return Start.AddDays(Nights); } }
        public DateTime EndWithPreparation { get { return Start.AddDays(Nights + PreparationDays); } }

        public Booking Clone()
        {
            return new Booking
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
    }
}
