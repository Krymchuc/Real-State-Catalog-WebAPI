using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_State_Catalog.Models
{
    public class HouseRules
    {
        [Key]
        [ForeignKey("Accommodation")]
        public Guid Id { get; set; }

        public virtual Accommodation? Accommodation { get; set; }

        [Display(Name = "Minimum arrival time")]
        [Required(ErrorMessage = "You must indicate the minimum arrival time")]
        [DataType(DataType.Time)]
        public TimeSpan ArrivalHour { get; set; }

        [Display(Name = "Maximum departure time")]
        [Required(ErrorMessage = "You must indicate the maximum departure time")]
        [DataType(DataType.Time)]
        public TimeSpan DepartureHour { get; set; }

        [Display(Name = "Animals allowed")]
        public bool PetAllowed { get; set; }

        [Display(Name = "Authorized parties")]
        public bool PartyAllowed { get; set; }

        [Display(Name = "Smoking accommodation")]
        public bool SmokeAllowed { get; set; }

        public override string ToString()
        {
            return "Animals : " + (PetAllowed ? "Yes" : "Non") + " -- Party : " + (PartyAllowed ? "Yes" : "Non") + " -- Smoking : " + (SmokeAllowed ? "Yes" : "Non")
                + " || Arrival time : " + ArrivalHour.ToString("hh\\hmm") + " -- Departure time : " + DepartureHour.ToString("hh\\hmm");
        }
    }
}
