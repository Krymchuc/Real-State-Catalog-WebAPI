using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Real_State_Catalog.Data;
using Real_State_Catalog.Models;

namespace Real_State_Catalog.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {

        private readonly AppContextDB _context;

        public FilterController(AppContextDB context)
        {
            _context = context;
        }

        // GET: api/<FilterController>
        [HttpGet("{city}/{Type}/{nbPerson}/{PricePerNight}/")]
        public async Task<IEnumerable<Offer>> Get(string city, string Type, string nbPerson, string PricePerNight)
        {
            IEnumerable<Offer>? offers = null;

            string type = Type;
            int nbPersonInt = int.Parse(nbPerson);
            double PricePerNightDouble = double.Parse(PricePerNight);

            if (!city.Equals(""))
            {
                offers = await _context.Offers

                    .Where(o => o.Accommodation.Address.City == city && o.Accommodation.Type == type && o.Accommodation.MaxTraveler >= nbPersonInt)
                    .Where(o => o.PricePerNight <= PricePerNightDouble)
                    //.Include(o => o.Accommodation.Pictures)
                    //Include(o => o.Accommodation.Address)
                    .Select(o => new Offer
                    {
                        Id = o.Id,
                        AddingDateTime = o.AddingDateTime,
                        StartAvailability = o.StartAvailability,
                        EndAvailability = o.EndAvailability,
                        PricePerNight = o.PricePerNight,
                        CleaningFee = o.CleaningFee,

                        Accommodation = new Accommodation
                        {
                            Name = o.Accommodation.Name,
                            Type = o.Accommodation.Type,
                            Description = o.Accommodation.Description,
                            MaxTraveler = o.Accommodation.MaxTraveler,

                            Address = new Address
                            {
                                City = o.Accommodation.Address.City,
                                Country = o.Accommodation.Address.Country
                            },

                            Pictures = o.Accommodation.Pictures
                        }
                    })
                    .ToListAsync();
            }

            return offers;
        }
    }
}
