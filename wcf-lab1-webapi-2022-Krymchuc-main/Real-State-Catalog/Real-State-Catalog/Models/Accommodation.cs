using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Real_State_Catalog.Models
{
    public class Accommodation
    {
        [Key]
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        [Display(Name = "User")]
        [JsonIgnore]
        public virtual User? User { get; set; }
        [Display(Name = "Offers")]
        [JsonIgnore]
        public virtual List<Offer>? Offers { get; set; }

        [Display(Name = "Address")]
        public virtual Address? Address { get; set; }

        [Display(Name = "House Rules")]
        public virtual HouseRules? HouseRules { get; set; }

        [Display(Name = "Photos")]
        public virtual List<Picture>? Pictures { get; set; }

        [Display(Name = "Rooms")]
        public virtual List<Room>? Rooms { get; set; }

        [Required(ErrorMessage = "You must enter a name for your accommodation")]
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "You must select the housing type")]
        [Display(Name = "Type")]
        [RegularExpression("Apartment|House|Room in apartment|Room in house", ErrorMessage = "Please select a valid slot type")]
        public string? Type { get; set; }
        [Required(ErrorMessage = "You must specify the maximum number of travelers")]
        [Display(Name = "Maximum Travelers")]
        public int MaxTraveler { get; set; }
        [Required(ErrorMessage = "You must enter the description of your accommodation")]
        public string? Description { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        [NotMapped]
        public string? LongitudeRaw { get; set; }
        [NotMapped]
        public string? LatitudeRaw { get; set; }
    }
}
        