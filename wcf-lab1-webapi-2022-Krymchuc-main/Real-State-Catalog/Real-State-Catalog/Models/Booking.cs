using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Real_State_Catalog.Models
{
    public class Booking
    {
        [Key]
        
        public Guid? Id { get; set; }

        public Guid? OfferId { get; set; }

        [Display(Name = "Offer")]
        public virtual Offer Offer { get; set; }

        public string UserId { get; set; }

        [Display(Name = "User")]
        public virtual User User { get; set; }

        [Display(Name = "Reservation Date")]
        [DataType(DataType.DateTime)]
        public DateTime BookingDateTime { get; set; }

        [Display(Name = "Arrival Date")]
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }

        [Display(Name = "Arrival time")]
        [DataType(DataType.Time)]
        public TimeSpan ArrivalTime { get; set; }

        [Display(Name = "Departure Date")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [Display(Name = "Departure Time")]
        [DataType(DataType.Time)]
        public TimeSpan DepartureTime { get; set; }

        [Display(Name = "Number of travelers")]
        public int NbPerson { get; set; }

        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }

        public Booking()
        {
            this.BookingDateTime = DateTime.Now;
        }
    }
}