using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Domain
{
    public class Booking : IBooking
    {
        public int RentalUnits { get; set; }
        public int PreparationTime { get; set; }
        public ResourceIdViewModel CreateNewBooking(BookingBindingModel bookingRequest, IDictionary<int, BookingViewModel> bookings)
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

        public bool CheckAvailability(BookingBindingModel bookingRequest, IDictionary<int, BookingViewModel> bookings)
        {
            return bookings.Where(
                booking => booking.Value.RentalId == bookingRequest.RentalId
                           && (booking.Value.Start <= bookingRequest.Start.Date 
                                && booking.Value.Start.AddDays(booking.Value.Nights + PreparationTime) > bookingRequest.Start.Date)// If a booking match with date and number of nights
                           || (booking.Value.Start < bookingRequest.Start.AddDays(bookingRequest.Nights) // If bookings starts before the given nights
                                && booking.Value.Start.AddDays(booking.Value.Nights + PreparationTime) >= bookingRequest.Start.AddDays(bookingRequest.Nights))// If nights are higgers than the requested
                           || (booking.Value.Start > bookingRequest.Start 
                                && booking.Value.Start.AddDays(booking.Value.Nights + PreparationTime) < bookingRequest.Start.AddDays(bookingRequest.Nights)))// The booked days are after the requested but the nights match
                .Count() >= RentalUnits;

        }
    }
}
