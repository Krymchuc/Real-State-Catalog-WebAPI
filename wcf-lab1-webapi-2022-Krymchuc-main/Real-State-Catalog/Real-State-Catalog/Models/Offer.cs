using System.ComponentModel.DataAnnotations;

namespace Real_State_Catalog.Models
{
    public class Offer
    {
		[Key]
		public Guid Id { get; set; }

		public Guid AccommodationId { get; set; }
		[Display(Name = "Lodging")]
		public virtual Accommodation? Accommodation { get; set; }

		[Display(Name = "Date Added")]
		[DataType(DataType.DateTime)]
		public DateTime AddingDateTime { get; set; }

		[Display(Name = "Start availability")]
		[DataType(DataType.Date)]
		public DateTime StartAvailability { get; set; }

		[Display(Name = "End of availability")]
		[DataType(DataType.Date)]
		public DateTime EndAvailability { get; set; }

		[Display(Name = "Price per night")]
		public double PricePerNight { get; set; }

		[Display(Name = "Cleaning fee")]
		public double CleaningFee { get; set; }

		public Offer()
		{
			this.AddingDateTime = DateTime.Now;
		}
	}
}
