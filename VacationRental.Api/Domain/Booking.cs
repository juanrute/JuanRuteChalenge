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

        internal int CheckAvailability(BookingBindingModel bookingRequest, IDictionary<int, BookingViewModel> bookings, int preparationTime)
        {
            return bookings.Where(
                booking => booking.Value.RentalId == bookingRequest.RentalId
                           && (booking.Value.Start <= bookingRequest.Start.Date 
                                && booking.Value.Start.AddDays(booking.Value.Nights + preparationTime) > bookingRequest.Start.Date)// si hay un booking que coincide con la fecha para la cantidad de noches
                           || (booking.Value.Start < bookingRequest.Start.AddDays(bookingRequest.Nights) // si el boonking inicio antes de las noches en cuestion
                                && booking.Value.Start.AddDays(booking.Value.Nights + preparationTime) >= bookingRequest.Start.AddDays(bookingRequest.Nights))// y las noches son mas de la que se quieren ahora
                           || (booking.Value.Start > bookingRequest.Start 
                                && booking.Value.Start.AddDays(booking.Value.Nights + preparationTime) < bookingRequest.Start.AddDays(bookingRequest.Nights)))// la fecha de booking es despues de la nueva fecha pero las noches coinciden
                .Count();

        }
    }
}
