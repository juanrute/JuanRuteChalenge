using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private IBooking _bookingDomain;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings,
            IBooking bookingDomain)
        {
            _rentals = rentals;
            _bookings = bookings;
            _bookingDomain = bookingDomain;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                throw new ApplicationException("Booking not found");

            return _bookings[bookingId];
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel bookingRequest)
        {
            if (bookingRequest.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
            if (!_rentals.ContainsKey(bookingRequest.RentalId))
                throw new ApplicationException("Rental not found");

            _bookingDomain.RentalUnits = _rentals[bookingRequest.RentalId].Units;
            if(_bookingDomain.CheckAvailability(bookingRequest, _bookings))
                throw new ApplicationException("Not available");

            _bookingDomain.PreparationTime = _rentals[bookingRequest.RentalId].PreparationTimeInDays;
            return _bookingDomain.CreateNewBooking(bookingRequest,_bookings);
        }
    }
}
