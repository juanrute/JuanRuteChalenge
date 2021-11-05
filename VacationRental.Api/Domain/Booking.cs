using System;
using System.Collections.Generic;
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

        internal void CheckAvailability(BookingBindingModel bookingRequest, IDictionary<int, BookingViewModel> bookings, int rentalUnits)
        {
            for (var i = 0; i < bookingRequest.Nights; i++)
            {
                var usedUnits = 0;
                foreach (var booking in bookings.Values)
                {
                    if (booking.RentalId == bookingRequest.RentalId
                        && (booking.Start <= bookingRequest.Start.Date && booking.Start.AddDays(booking.Nights) > bookingRequest.Start.Date)
                        || (booking.Start < bookingRequest.Start.AddDays(bookingRequest.Nights) && booking.Start.AddDays(booking.Nights) >= bookingRequest.Start.AddDays(bookingRequest.Nights))
                        || (booking.Start > bookingRequest.Start && booking.Start.AddDays(booking.Nights) < bookingRequest.Start.AddDays(bookingRequest.Nights)))
                    {
                        usedUnits++;
                    }
                }
                if (usedUnits >= rentalUnits)
                    throw new ApplicationException("Not available");
            }
        }
    }
}
