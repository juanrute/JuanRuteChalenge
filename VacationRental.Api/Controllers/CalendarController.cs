using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Domain;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;
        private readonly ICalendar _calendarDomain;

        public CalendarController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings,
            IDictionary<int, int> assignedUnits,
            ICalendar calendar)
        {
            _rentals = rentals;
            _bookings = bookings;
            _calendarDomain = calendar;
            _calendarDomain.AssignedUnits = assignedUnits;
        }

        [HttpGet]
        public CalendarViewModel Get(
            int rentalId, DateTime start, 
            int nights)
        {
            var calendarRequest = new CalendarBindingModel
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights
            };

            if (calendarRequest.Nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.ContainsKey(calendarRequest.RentalId))
                throw new ApplicationException("Rental not found");

            _calendarDomain.PreparationTime = _rentals[calendarRequest.RentalId].PreparationTimeInDays;

            return _calendarDomain.CreateCalendarResponse(calendarRequest, _bookings);
        }
    }
}
