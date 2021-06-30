using VacationRental.Domain.Rental;

namespace VacationRental.Api.Models
{
    public class RentalBindingModel
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }

        internal Rental ToRental()
        {
            return new Rental
            {
                Units = this.Units,
                PreparationTimeInDays = this.PreparationTimeInDays
            };
        }

    }
}
