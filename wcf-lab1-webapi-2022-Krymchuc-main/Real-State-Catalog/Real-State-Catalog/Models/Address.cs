using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Real_State_Catalog.Models
{
    public class Address
    {
		[Key]
		[ForeignKey("Accommodation")]
		public Guid Id { get; set; }

		[JsonIgnore] 
		public virtual Accommodation? Accommodation { get; set; }

		[Required(ErrorMessage = "You must enter the number and street of your accommodation")]
		[Display(Name = "Number and Street")]
		public String? StreetAndNumber { get; set; }

		[Display(Name = "Address Complement")]
		public String? Complement { get; set; }

		[Required(ErrorMessage = "You must enter the postal code of your accommodation")]
		[Display(Name = "Postcode")]
		public String? PostalCode { get; set; }

		[Required(ErrorMessage = "You must enter the city of your accommodation")]
		[Display(Name = "City")]
		public String? City { get; set; }

		[Required(ErrorMessage = "You must enter the country of your accommodation")]
		[Display(Name = "Country")]
		public String? Country { get; set; }

		public override String ToString()
        {
			return StreetAndNumber + ", " + Complement + "\n" + PostalCode + " " + City + ", " + Country;
        }

		public String ShortAddress()
        {
			return City + ", " + Country;
		}
	}
}
