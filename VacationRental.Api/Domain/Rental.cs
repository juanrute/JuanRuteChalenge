using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Domain
{
    public class Rental : IRental
    {
        public int RentalId { get; set; }

        public bool Validator(
            RentalBindingModel rentalRequest, 
            RentalViewModel rental, 
            IDictionary<int, BookingViewModel> Bookings)
        {
            //TODO: Validate if with the changes it will overlap between existing bookings 
            if (Bookings.Where(booking => booking.Value.RentalId == RentalId).Count() <= 1)
            {
                return true;
            }
            else if (rental.Units <= rentalRequest.Units && rental.PreparationTimeInDays == rentalRequest.PreparationTimeInDays)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
