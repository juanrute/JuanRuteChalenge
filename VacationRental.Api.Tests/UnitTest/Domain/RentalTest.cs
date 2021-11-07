using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Api.Domain;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests.UnitTest.Domain
{
    public class RentalTest : IDisposable
    {
        IDictionary<int, BookingViewModel> bookings;
        IRental rentalDomain;
        public RentalTest()
        {
            bookings = new Dictionary<int, BookingViewModel>();
            rentalDomain = new Rental();
            rentalDomain.RentalId = 1;
        }
        public void Dispose()
        {
            bookings.Clear();
        }

        [Fact]
        public void Validator_WhenNoBookings_ReturnTrue()
        {
            var rentalRequest = new RentalBindingModel
            {
                Units = 10,
                PreparationTimeInDays = 10
            };
            var currentRental = new RentalViewModel
            {
                Id = 1,
                Units = 2,
                PreparationTimeInDays = 2
            };
            Assert.True(rentalDomain.Validator(rentalRequest,currentRental, bookings));
        }

        [Fact]
        public void Validator_WhenPreparationTimeIsTheSame_ReturnTrue()
        {
            var rentalRequest = new RentalBindingModel
            {
                Units = 10,
                PreparationTimeInDays = 2
            };

            bookings.Add(1, new BookingViewModel
            {
                Id = 1,
                RentalId = 1,
                Start = new DateTime(2002, 01, 01),
                Nights = 2
            });
            bookings.Add(2, new BookingViewModel
            {
                Id = 2,
                RentalId = 1,
                Start = new DateTime(2002, 01, 01),
                Nights = 1
            });

            var currentRental = new RentalViewModel
            {
                Id = 1,
                Units = 2,
                PreparationTimeInDays = 2
            };
            Assert.True(rentalDomain.Validator(rentalRequest, currentRental, bookings));
        }

        [Fact]
        public void Validator_WhenOverlapp_ReturnFalse()
        {
            var rentalRequest = new RentalBindingModel
            {
                Units = 10,
                PreparationTimeInDays = 4
            };

            bookings.Add(1, new BookingViewModel
            {
                Id = 1,
                RentalId = 1,
                Start = new DateTime(2002, 01, 01),
                Nights = 2
            });
            bookings.Add(2, new BookingViewModel
            {
                Id = 2,
                RentalId = 1,
                Start = new DateTime(2002, 01, 01),
                Nights = 1
            });

            var currentRental = new RentalViewModel
            {
                Id = 1,
                Units = 2,
                PreparationTimeInDays = 2
            };
            Assert.False(rentalDomain.Validator(rentalRequest, currentRental, bookings));
        }
    }
}
