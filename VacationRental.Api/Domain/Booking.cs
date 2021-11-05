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

        internal void CheckAvailability(BookingBindingModel bookingRequest, IDictionary<int, BookingViewModel> bookings, int rentalUnits)
        {
            var usedUnitsBetter = bookings.Where(x => x.Value.RentalId == bookingRequest.RentalId
                                            && (x.Value.Start <= bookingRequest.Start.Date && x.Value.Start.AddDays(x.Value.Nights) > bookingRequest.Start.Date)
                                            || (x.Value.Start < bookingRequest.Start.AddDays(bookingRequest.Nights) && x.Value.Start.AddDays(x.Value.Nights) >= bookingRequest.Start.AddDays(bookingRequest.Nights))
                                            || (x.Value.Start > bookingRequest.Start && x.Value.Start.AddDays(x.Value.Nights) < bookingRequest.Start.AddDays(bookingRequest.Nights))
                                            ).Sum(book => book.Value.Nights);


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
