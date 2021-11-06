using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Domain
{
    public interface ICalendar
    {
        int PreparationTime { get; set; }
        IDictionary<int, int> AssignedUnits { get; set; }
        CalendarViewModel CreateCalendarResponse(CalendarBindingModel calendarRequest, IDictionary<int, BookingViewModel> Bookings);
    }
}
