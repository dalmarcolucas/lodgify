using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using VacationRental.Infrastructure.Repositories;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private IRentalRepository _rentalRepository;
        private RentalService _rentalService;


        private RentalService RentalService
        {
            get { return _rentalService ?? (_rentalService = new RentalService(_rentalRepository)); }
        }

        public RentalsController(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
           return new RentalViewModel(_rentalRepository.GetRental(rentalId));
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            return new ResourceIdViewModel()
            {
                Id = _rentalRepository.AddRental(model.ToRental()).Id
            };
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public RentalViewModel Put(int rentalId, RentalBindingModel model)
        {
            RentalService.UpdateRental(rentalId, model);

            return new RentalViewModel(_rentalRepository.GetRental(rentalId));
        }

    }
}
