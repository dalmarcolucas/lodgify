using System;
using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class CalendarDateViewModel
    {
        private List<CalendarBookingViewModel> _bookings;
        private List<CalendarPreparationTimesViewModel> _preparationTimes;

        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get => _bookings ?? (_bookings = new List<CalendarBookingViewModel>()); set => _bookings = value; }
        public List<CalendarPreparationTimesViewModel> PreparationTimes { get => _preparationTimes ?? (_preparationTimes = new List<CalendarPreparationTimesViewModel>()); set => _preparationTimes = value; }
    }
}
