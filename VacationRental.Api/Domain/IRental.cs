using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Domain
{
    public interface IRental
    {
        int RentalId { get; set; }
        bool Validator(RentalBindingModel rentalRequest, RentalViewModel rental, IDictionary<int, BookingViewModel> Bookings);
    }
}
