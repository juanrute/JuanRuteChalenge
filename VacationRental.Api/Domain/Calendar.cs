using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Domain
{
    public class Calendar
    {
        internal CalendarViewModel CreateCalendarResponse(int rentalId, DateTime start, int nights, IDictionary<int, BookingViewModel> bookings)
        {
            var resultCalendar = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            for (var i = 0; i < nights; i++)
            {
                var dateTarget = start.Date.AddDays(i);
                var date = new CalendarDateViewModel
                {
                    Date = dateTarget,
                    Bookings = bookings
                    .Where(
                        booking => booking.Value.RentalId == rentalId
                        && booking.Value.Start <= dateTarget 
                        && booking.Value.Start.AddDays(booking.Value.Nights) > dateTarget)
                    .Select(
                        booking => new CalendarBookingViewModel { Id = booking.Value.Id })
                    .ToList()
                };

                resultCalendar.Dates.Add(date);
            }

            return resultCalendar;
        }
    }
}
