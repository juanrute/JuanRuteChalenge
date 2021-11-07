using Xunit;
using System;
using VacationRental.Api.Models;
using System.Collections.Generic;
using VacationRental.Api.Domain;

namespace VacationRental.Api.Tests.UnitTest.Domain
{
    public class BookingTests : IDisposable
    {
        IDictionary<int, BookingViewModel> bookings;
        IBooking bookingDomain;

        public BookingTests()
        {
            bookings = new Dictionary<int, BookingViewModel>();
            bookingDomain = new Booking();
            bookingDomain.PreparationTime = 1;
            bookingDomain.RentalUnits = 1;
        }

        public void Dispose()
        {
            bookings.Clear();
        }

        [Fact]
        public void CheckAvailability_WhenThereAre_ReturnTrue()
        {
            var bookingRequest = new BookingBindingModel
            {
                RentalId = 1,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };
            
            Assert.False(bookingDomain.CheckUnavailability(bookingRequest, bookings));
        }

        [Fact]
        public void CheckAvailability_WhenThereAreNot_ReturnFalse()
        {
            //Arrange
            var bookingRequest = new BookingBindingModel
            {
                RentalId = 1,
                Nights = 3,
                Start = new DateTime(2001, 01, 01)
            };
            bookings.Add(1, new BookingViewModel
            {
                Id = 1,
                RentalId = bookingRequest.RentalId,
                Start = bookingRequest.Start,
                Nights = 2
            });

            //Act
            var result = bookingDomain.CheckUnavailability(bookingRequest, bookings);

            //Assert
            Assert.True(result);
        }
    }
}
