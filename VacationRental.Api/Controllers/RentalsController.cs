using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Domain;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly IRental _rental;

        public RentalsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings,
            IRental rental)
        {
            _rentals = rentals;
            _bookings = bookings;
            _rental = rental;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(
            int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            return _rentals[rentalId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(
            RentalBindingModel rentalRequest)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = rentalRequest.Units,
                PreparationTimeInDays = rentalRequest.PreparationTimeInDays
            });

            return key;
        }

        [HttpPut("{rentalId:int}")]
        public RentalViewModel Put(
            int rentalId, 
            RentalBindingModel rentalRequest)
        {
            if (_rental.Validator(rentalRequest, _rentals[rentalId], _bookings))
            {
                _rentals[rentalId] = new RentalViewModel { 
                    Id = rentalId,
                    Units = rentalRequest.Units,
                    PreparationTimeInDays = rentalRequest.PreparationTimeInDays
                };
            }
            else
            {
                throw new ApplicationException("Rental cannot be updated");
            }

            return _rentals[rentalId];
        }
    }
}
