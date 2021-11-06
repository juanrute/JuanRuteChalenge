using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Domain
{
    public interface IBooking
    {
        int RentalUnits { get; set; }
        int PreparationTime { get; set; }
        bool CheckAvailability(BookingBindingModel bookingRequest, IDictionary<int, BookingViewModel> Bookings);
        ResourceIdViewModel CreateNewBooking(BookingBindingModel bookingRequest, IDictionary<int, BookingViewModel> bookings);
    }
}
