using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IRentalRepository _rentalRepository;
        private CalendarService _calendarService;

        private CalendarService CalendarService
        {
            get { return _calendarService ?? (_calendarService = new CalendarService(_rentalRepository)); }
        }

        public CalendarController(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            CalendarViewModel result = CalendarService.GetCalendar(rentalId, start, nights);

            return result;
        }
    }
}
