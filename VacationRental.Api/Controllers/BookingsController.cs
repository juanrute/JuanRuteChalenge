using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
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

            CheckAvailability(bookingRequest);

            return CreateNewBooking(bookingRequest);
        }

        private ResourceIdViewModel CreateNewBooking(BookingBindingModel bookingRequest)
        {
            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = bookingRequest.Nights,
                RentalId = bookingRequest.RentalId,
                Start = bookingRequest.Start.Date
            });

            return key;
        }

        private void CheckAvailability(BookingBindingModel bookingRequest)
        {
            for (var i = 0; i < bookingRequest.Nights; i++)
            {
                var usedUnits = 0;
                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == bookingRequest.RentalId
                        && (booking.Start <= bookingRequest.Start.Date && booking.Start.AddDays(booking.Nights) > bookingRequest.Start.Date)
                        || (booking.Start < bookingRequest.Start.AddDays(bookingRequest.Nights) && booking.Start.AddDays(booking.Nights) >= bookingRequest.Start.AddDays(bookingRequest.Nights))
                        || (booking.Start > bookingRequest.Start && booking.Start.AddDays(booking.Nights) < bookingRequest.Start.AddDays(bookingRequest.Nights)))
                    {
                        usedUnits++;
                    }
                }
                if (usedUnits >= _rentals[bookingRequest.RentalId].Units)
                    throw new ApplicationException("Not available");
            }
        }
    }
}
