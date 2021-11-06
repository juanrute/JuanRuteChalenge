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

        public CalendarViewModel CreateCalendarResponse(
            CalendarBindingModel calendarRequest, 
            IDictionary<int, BookingViewModel> Bookings)
        {
            var resultCalendar = new CalendarViewModel
            {
                RentalId = calendarRequest.RentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            int assignedUnit = 1;
            for (var indexNight = 0; indexNight < calendarRequest.Nights; indexNight++)
            {
                var dateTarget = calendarRequest.Start.Date.AddDays(indexNight);
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
                                return new CalendarBookingViewModel { 
                                    Id = booking.Value.Id, 
                                    Unit = AssignUnitInOrder(booking, ref assignedUnit) 
                                };
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
                                return new CalendarPreparationViewModel { 
                                    Unit = AssignUnitInOrder(booking, ref assignedUnit) 
                                };
                            })
                        .ToList()
                };
                
                resultCalendar.Dates.Add(date);
            }

            return resultCalendar;
        }

        private int AssignUnitInOrder(KeyValuePair<int, BookingViewModel> booking, ref int assignedUnit)
        {
            if (!AssignedUnits.TryGetValue(booking.Value.Id, out int value))
            {
                value = assignedUnit++;
                if (value > PreparationTime)
                    value = 1;
                AssignedUnits.Add(booking.Value.Id, value);
            }
            return value;
        }
    }
}
