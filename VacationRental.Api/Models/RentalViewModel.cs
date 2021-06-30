using VacationRental.Domain.Rental;

namespace VacationRental.Api.Models
{
    public class RentalViewModel
    {
        public RentalViewModel()
        {
        }

        public RentalViewModel(Rental rental)
        {
            this.Id = rental.Id;
            this.Units = rental.Units;
            this.PreparationTimeInDays = rental.PreparationTimeInDays;
        }

        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
