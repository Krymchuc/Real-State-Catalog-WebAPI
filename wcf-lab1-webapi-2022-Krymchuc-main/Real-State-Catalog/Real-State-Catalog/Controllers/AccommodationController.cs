using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Real_State_Catalog.Data;
using Real_State_Catalog.Models;

namespace Real_State_Catalog.Controllers
{
    [Authorize(Roles = "Admin,Host")]
    public class AccommodationController : Controller
    {     
            private readonly AppContextDB _context;
            private readonly UserManager<User> _userManager;
            private readonly IWebHostEnvironment _environment;

            public AccommodationController(AppContextDB context, UserManager<User> userManager, IWebHostEnvironment environment)
            {
                _context = context;
                _userManager = userManager;
                _environment = environment;
            }

            // GET: Accommodation
            public async Task<IActionResult> Index()
            {
                User user = await _userManager.GetUserAsync(User);

                if (user == null) { return NotFound(); }

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return View(await _context.Accommodations
                        .Include(a => a.Address).Include(a => a.User).ToListAsync());
                }
                else
                {
                    return View(await _context.Accommodations
                        .Where(a => a.UserId == user.Id)
                        .Include(a => a.Address).Include(a => a.User).ToListAsync());
                }
            }

            // GET: Accommodation/Details
            public async Task<IActionResult> Details(Guid? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var accommodation = await _context.Accommodations
                    .Include(a => a.Offers)
                    .Include(a => a.Address)
                    .Include(a => a.User)
                    .Include(a => a.Pictures)
                    .Include(a => a.HouseRules)
                    .Include(a => a.Rooms)
                    .ThenInclude(r => r.Amenities)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (accommodation == null)
                {
                    return NotFound();
                }

                return View(accommodation);
            }

            // GET: Accommodation/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Accommodation/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(
                [Bind("Name, Type, MaxTraveler, Description, LatitudeRaw, LongitudeRaw")] Accommodation accommodation,
                [Bind("StreetAndNumber, Complement, City, PostalCode, Country")] Address address,
                [Bind("ArrivalHour, DepartureHour, PetAllowed, PartyAllowed, SmokeAllowed")] HouseRules houseRules)
            {
                if (!ModelState.IsValid) 
            { return View(accommodation); }
                accommodation.Latitude=double.Parse(accommodation.LatitudeRaw.Replace(".",","));
                accommodation.Longitude=double.Parse(accommodation.LongitudeRaw.Replace(".", ","));  

                accommodation.UserId = (await _userManager.GetUserAsync(User)).Id;
                accommodation.Address = address;
                accommodation.HouseRules = houseRules;

                _context.Add(accommodation);
                await _context.SaveChangesAsync();

                return RedirectToAction("ManagePictures", "Picture", new { id = accommodation.Id });
            }

            // GET: Accommodation/Edit
            public async Task<IActionResult> Edit(Guid? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var accommodation = await _context.Accommodations
                    .Include(a => a.Address)
                    .Include(a => a.HouseRules)
                    .Include(a => a.Pictures)
                    .Include(a => a.Rooms)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (accommodation == null)
                {
                    return NotFound();
                }
                return View(accommodation);
            }

            // POST: Accommodation/Edit
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(Guid id,
                [Bind("Id, Name, Type, MaxTraveler, Description,  LatitudeRaw, LongitudeRaw")] Accommodation accommodation,
                [Bind("Id, StreetAndNumber, Complement, City, PostalCode, Country")] Address address,
                [Bind("Id, ArrivalHour, DepartureHour, PetAllowed, PartyAllowed, SmokeAllowed")] HouseRules houseRules)
            {
                if (id != accommodation.Id)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid) { return View(accommodation); }

                accommodation.Latitude = double.Parse(accommodation.LatitudeRaw.Replace(".", ","));
                accommodation.Longitude = double.Parse(accommodation.LongitudeRaw.Replace(".", ","));

                accommodation.UserId = await _context.Accommodations.Where(a => a.Id == id).Select(a => a.UserId).SingleOrDefaultAsync();
                accommodation.Address = address;
                accommodation.HouseRules = houseRules;
                try
                {
                    _context.Update(accommodation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccommodationExists(accommodation.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction("Edit", new { id });
            }

            // GET: Accommodation/Delete
            public async Task<IActionResult> Delete(Guid? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var accommodation = await _context.Accommodations
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (accommodation == null)
                {
                    return NotFound();
                }

                return View(accommodation);
            }

            // POST: Accommodation/Delete
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(Guid id)
            {
                var accommodation = await _context.Accommodations.FindAsync(id);
                _context.Accommodations.Remove(accommodation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool AccommodationExists(Guid id)
            {
                return _context.Accommodations.Any(e => e.Id == id);
            }
    }
}
