using System.ComponentModel.DataAnnotations;

namespace Real_State_Catalog.Models
{
    public enum RoomTypes
    {
        Bedroom,
        Bathroom,
        Kitchen
    }

    public class Room
    {
        [Key]
        public Guid Id { get; set; }

        public Guid AccommodationId { get; set; }

        public RoomTypes RoomType { get; set; }

        [Display(Name = "Equipement(s)")]
        public virtual List<Amenity>? Amenities { get; set; }

        public string AmenitiesStr()
        {
            return String.Join(", ", Amenities);
        }
    }

    public abstract class TypeTranslate
    {
        public static string ToFr(string roomType)
        {
            return roomType switch
            {
                "Bedroom" => "Bedroom",
                "Bathroom" => "Bathroom",
                "Kitchen" => "Kitchen",
                _ => null,
            };
        }
    }
}
