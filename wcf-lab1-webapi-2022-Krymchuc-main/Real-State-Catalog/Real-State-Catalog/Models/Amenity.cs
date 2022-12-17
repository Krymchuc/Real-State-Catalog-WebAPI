using System.ComponentModel.DataAnnotations;

namespace Real_State_Catalog.Models
{
    public enum AmenityTypes
    {
        SingleBed,
        DoubleBed,
        TV,
        Closet,
        Bathtub,
        Shower,
        WashingMachine,
        Oven,
        Freezer,
        CoffeeMaker
    }

    public class Amenity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public AmenityTypes AmenityType { get; set; }

        public override string ToString()
        {
            return AmenityTools.ToFr(Enum.GetName(typeof(AmenityTypes), this.AmenityType));
        }
    }

    public static class AmenityTools
    {
        public static List<string> AmenitiesForRoom(string roomType)
        {
            return roomType switch
            {
                "Bedroom" => new List<string> { "SingleBed", "DoubleBed", "TV", "Closet" },
                "Bathroom" => new List<string> { "Bathtub", "Shower", "WashingMachine" },
                "Kitchen" => new List<string> { "Oven", "Freezer", "CoffeeMaker" },
                _ => null,
            };
        }

        public static string ToFr(string amenityType)
        {
            return amenityType switch
            {
                "SingleBed" => "SingleBed",
                "DoubleBed" => "DoubleBed",
                "TV" => "TV",
                "Closet" => "Closet",
                "Bathtub" => "Bathtub",
                "Shower" => "Shower",
                "WashingMachine" => "WashingMachine",
                "Oven" => "Oven",
                "Freezer" => "Freezer",
                "CoffeeMaker" => "CoffeeMaker",
                _ => null,
            };
        }
    }
}
