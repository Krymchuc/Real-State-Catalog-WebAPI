using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Real_State_Catalog.Models
{
    public class User: IdentityUser
    {
        [PersonalData]
        [Display(Name = "FirstName")]
        public String? FirstName { get; set; }

        [PersonalData]
        [Display(Name = "Name")]
        public String? LastName { get; set; }

        [Display(Name = "Slot(s)")]
        public virtual List<Accommodation>? Accommodations { get; set; }

        [Display(Name = "Favorites")]
        public virtual List<Bookmark>? Bookmarks { get; set; }
    }
}
