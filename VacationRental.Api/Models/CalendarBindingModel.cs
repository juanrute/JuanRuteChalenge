using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.Api.Models
{
    public class CalendarBindingModel
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public int Nights { get; set; }
    }
}
