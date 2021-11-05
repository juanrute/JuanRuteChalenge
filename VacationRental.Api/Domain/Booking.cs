using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Domain
{
    public class Booking
    {

        internal ResourceIdViewModel CreateNewBooking(BookingBindingModel bookingRequest, IDictionary<int, BookingViewModel> bookings)
        {
            var key = new ResourceIdViewModel { Id = bookings.Keys.Count + 1 };

            bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = bookingRequest.Nights,
                RentalId = bookingRequest.RentalId,
                Start = bookingRequest.Start.Date
            });

            return key;
        }

        internal int CheckAvailability(BookingBindingModel bookingRequest, IDictionary<int, BookingViewModel> bookings)
        {
            return bookings.Where(
                booking => booking.Value.RentalId == bookingRequest.RentalId
                           && (booking.Value.Start <= bookingRequest.Start.Date 
                                && booking.Value.Start.AddDays(booking.Value.Nights) > bookingRequest.Start.Date)
                           || (booking.Value.Start < bookingRequest.Start.AddDays(bookingRequest.Nights) 
                                && booking.Value.Start.AddDays(booking.Value.Nights) >= bookingRequest.Start.AddDays(bookingRequest.Nights))
                           || (booking.Value.Start > bookingRequest.Start 
                                && booking.Value.Start.AddDays(booking.Value.Nights) < bookingRequest.Start.AddDays(bookingRequest.Nights)))
                .Count();

        }
    }
}
