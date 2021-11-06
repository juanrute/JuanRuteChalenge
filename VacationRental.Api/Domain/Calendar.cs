using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Domain
{
    public class Calendar
    {
        IDictionary<int, int> _assignedUnits = new Dictionary<int, int>();
        internal CalendarViewModel CreateCalendarResponse(int rentalId, DateTime start, int nights, IDictionary<int, BookingViewModel> bookings, int preparationTime)
        {
            var resultCalendar = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            int assignedUnit = 1;
            for (var i = 0; i < nights ; i++)
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
                            booking => {
                                int value;
                                if(!_assignedUnits.TryGetValue(booking.Value.Id, out value)) {
                                    value = assignedUnit++;
                                    if (value > preparationTime)
                                        value = 1;
                                    _assignedUnits.Add(booking.Value.Id, value);
                                }

                                return new CalendarBookingViewModel { Id = booking.Value.Id, Unit = value };
                        })
                        .ToList(),
                    PreparationTimes = bookings
                        .Where(
                            booking => booking.Value.RentalId == rentalId
                            && booking.Value.Start.AddDays(booking.Value.Nights) <= dateTarget
                            && booking.Value.Start.AddDays(booking.Value.Nights + preparationTime) > dateTarget
                            )
                        .Select(
                            booking => {
                                int value;
                                if (!_assignedUnits.TryGetValue(booking.Value.Id, out value))
                                {
                                    value = assignedUnit++;
                                    if (value > preparationTime)
                                        value = 1;
                                    _assignedUnits.Add(booking.Value.Id, value);
                                }

                                return new CalendarPreparationViewModel { Unit = value };
                            })
                        .ToList()
                };
                
                resultCalendar.Dates.Add(date);
            }

            return resultCalendar;
        }
    }
}
