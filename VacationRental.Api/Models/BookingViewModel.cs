using System;

namespace VacationRental.Api.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public int Unit { get; set; }
        public int PreparationDays { get; set; }
        public DateTime End { get { return Start.AddDays(Nights); } }
        public DateTime EndWithPreparation { get { return Start.AddDays(Nights + PreparationDays); } }
    }
}
