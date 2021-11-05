using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Domain.Booking
{
    public class BookingDomain
    {


        private ResourceIdViewModel CreateNewBooking(BookingBindingModel bookingRequest)
        {
            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = bookingRequest.Nights,
                RentalId = bookingRequest.RentalId,
                Start = bookingRequest.Start.Date
            });

            return key;
        }

        private void CheckAvailability(BookingBindingModel bookingRequest)
        {
            for (var i = 0; i < bookingRequest.Nights; i++)
            {
                var usedUnits = 0;
                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId == bookingRequest.RentalId
                        && (booking.Start <= bookingRequest.Start.Date && booking.Start.AddDays(booking.Nights) > bookingRequest.Start.Date)
                        || (booking.Start < bookingRequest.Start.AddDays(bookingRequest.Nights) && booking.Start.AddDays(booking.Nights) >= bookingRequest.Start.AddDays(bookingRequest.Nights))
                        || (booking.Start > bookingRequest.Start && booking.Start.AddDays(booking.Nights) < bookingRequest.Start.AddDays(bookingRequest.Nights)))
                    {
                        usedUnits++;
                    }
                }
                if (usedUnits >= _rentals[bookingRequest.RentalId].Units)
                    throw new ApplicationException("Not available");
            }
        }
    }
}
