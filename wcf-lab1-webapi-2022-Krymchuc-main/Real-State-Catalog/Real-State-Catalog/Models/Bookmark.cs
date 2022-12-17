using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_State_Catalog.Models
{
    public class Bookmark
    {
        [Key]
        public Guid Id { get; set; }

        public string? UserId { get; set; }

        public Guid? OfferId { get; set; }
        [ForeignKey("OfferId")]
        public virtual Offer? Offer { get; set; }
    }
}
