using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Api.Domain;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests.UnitTest.Domain
{
    public class CalendarTest : IDisposable
    {
        ICalendar calendarDomain;
        IDictionary<int, BookingViewModel> bookings;

        public CalendarTest()
        {
            calendarDomain = new Calendar();
            calendarDomain.PreparationTime = 2;
            bookings = new Dictionary<int, BookingViewModel>();
            calendarDomain.AssignedUnits = new Dictionary<int, int>();
        }

        public void Dispose()
        {
            bookings.Clear();
        }

        [Fact]
        public void CreateCalendarResponse_WhenWeHaveSomeBookings_ReturnTheCorrectStructure()
        {
            //Arrange
            var calendarRequest = new CalendarBindingModel
            {
                RentalId = 1,
                Nights = 8,
                Start = new DateTime(2002, 01, 01)
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
            bookings.Add(3, new BookingViewModel
            {
                Id = 3,
                RentalId = 1,
                Start = new DateTime(2002, 01, 04),
                Nights = 2
            });
            //Act
            var result = calendarDomain.CreateCalendarResponse(calendarRequest, bookings);

            //Assert
            Assert.True(result.Dates.Count == 8);
            Assert.True(result.Dates[0].Bookings.Count == 2);
            Assert.Empty(result.Dates[0].PreparationTimes);

            Assert.True(result.Dates[1].Bookings.Count == 1);
            Assert.True(result.Dates[1].PreparationTimes.Count == 1);
            Assert.True(result.Dates[1].PreparationTimes[0].Unit == result.Dates[0].Bookings[1].Unit);

            Assert.True(result.Dates[2].Bookings.Count == 0);
            Assert.True(result.Dates[2].PreparationTimes.Count == 2);
            Assert.True(result.Dates[2].PreparationTimes[0].Unit == result.Dates[1].Bookings[0].Unit);
            Assert.True(result.Dates[2].PreparationTimes[1].Unit == result.Dates[0].Bookings[1].Unit);

            Assert.True(result.Dates[3].Bookings.Count == 1);
            Assert.True(result.Dates[3].PreparationTimes.Count == 1);
            Assert.True(result.Dates[3].PreparationTimes[0].Unit == result.Dates[1].Bookings[0].Unit);

            Assert.True(result.Dates[4].Bookings.Count == 1);
            Assert.True(result.Dates[4].PreparationTimes.Count == 0);

            Assert.True(result.Dates[5].Bookings.Count == 0);
            Assert.True(result.Dates[5].PreparationTimes.Count == 1);

            Assert.True(result.Dates[6].Bookings.Count == 0);
            Assert.True(result.Dates[6].PreparationTimes.Count == 1);

            Assert.True(result.Dates[7].Bookings.Count == 0);
            Assert.True(result.Dates[7].PreparationTimes.Count == 0);
        }

        [Fact]
        public void CreateCalendarResponse_WhenNoBookings_ReturnEmpty()
        {
            //Arrange
            var calendarRequest = new CalendarBindingModel
            {
                RentalId = 1,
                Nights = 4,
                Start = new DateTime(2002, 01, 01)
            };
            
            //Act
            var result = calendarDomain.CreateCalendarResponse(calendarRequest, bookings);

            //Assert
            Assert.True(result.Dates.Count == 4);
            Assert.True(result.Dates[0].Bookings.Count == 0);
            Assert.True(result.Dates[0].PreparationTimes.Count == 0);

            Assert.True(result.Dates[1].Bookings.Count == 0);
            Assert.True(result.Dates[1].PreparationTimes.Count == 0);

            Assert.True(result.Dates[2].Bookings.Count == 0);
            Assert.True(result.Dates[2].PreparationTimes.Count == 0);

            Assert.True(result.Dates[3].Bookings.Count == 0);
            Assert.True(result.Dates[3].PreparationTimes.Count == 0);
        }

    }
}
