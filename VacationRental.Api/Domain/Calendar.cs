using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Domain
{
    public class Calendar : ICalendar
    {
        public int PreparationTime { get; set; }
        public IDictionary<int, int> AssignedUnits { get; set; }
        public CalendarViewModel CreateCalendarResponse(CalendarBindingModel calendarRequest, IDictionary<int, BookingViewModel> Bookings)
        {
            var resultCalendar = new CalendarViewModel
            {
                RentalId = calendarRequest.RentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            int assignedUnit = 1;
            for (var i = 0; i < calendarRequest.Nights; i++)
            {
                var dateTarget = calendarRequest.Start.Date.AddDays(i);
                var date = new CalendarDateViewModel
                {
                    Date = dateTarget,
                    Bookings = Bookings
                        .Where(
                            booking => booking.Value.RentalId == calendarRequest.RentalId
                            && booking.Value.Start <= dateTarget 
                            && booking.Value.Start.AddDays(booking.Value.Nights) > dateTarget)
                        .Select(
                            booking => {
                                int value = AssignUnit(PreparationTime, booking, ref assignedUnit);
                                return new CalendarBookingViewModel { Id = booking.Value.Id, Unit = value };
                        })
                        .ToList(),
                    PreparationTimes = Bookings
                        .Where(
                            booking => booking.Value.RentalId == calendarRequest.RentalId
                            && booking.Value.Start.AddDays(booking.Value.Nights) <= dateTarget
                            && booking.Value.Start.AddDays(booking.Value.Nights + PreparationTime) > dateTarget
                            )
                        .Select(
                            booking => {
                                int value = AssignUnit(PreparationTime, booking, ref assignedUnit);
                                return new CalendarPreparationViewModel { Unit = value };
                            })
                        .ToList()
                };
                
                resultCalendar.Dates.Add(date);
            }

            return resultCalendar;
        }

        private int AssignUnit(int preparationTime, KeyValuePair<int, BookingViewModel> booking, ref int assignedUnit)
        {
            if (!AssignedUnits.TryGetValue(booking.Value.Id, out int value))
            {
                value = assignedUnit++;
                if (value > preparationTime)
                    value = 1;
                AssignedUnits.Add(booking.Value.Id, value);
            }

            return value;
        }
    }
}
