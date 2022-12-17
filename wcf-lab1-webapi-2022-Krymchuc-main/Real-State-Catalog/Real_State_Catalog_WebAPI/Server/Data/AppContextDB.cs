using Microsoft.EntityFrameworkCore;
using Real_State_Catalog_WebAPI.Models;

namespace Real_State_Catalog_WebAPI.Data
{
    public class AppContextDB : DbContext
    {
        public AppContextDB(DbContextOptions<AppContextDB> options) : base(options) { }

        public DbSet<Accommodation> Accommodations { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<Booking> Booking { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Amenity> Amenity { get; set; }

        public DbSet<Bookmark> Bookmark { get; set; }

    }
}
