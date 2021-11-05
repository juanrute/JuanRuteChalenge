using System;
using System.Collections.Generic;
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
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>()
                };

                foreach (var booking in bookings.Values)
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id });
                    }
                }

                resultCalendar.Dates.Add(date);
            }

            return resultCalendar;
        }
    }
}
