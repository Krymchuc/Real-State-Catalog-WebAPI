using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Real_State_Catalog_WebAPI.Data;
using Real_State_Catalog_WebAPI.Models;

namespace Real_State_Catalog_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Host")]
    public class OfferController : ControllerBase
    {
        private readonly AppContextDB _context;
        private readonly UserManager<User> _userManager;

        public OfferController(AppContextDB context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Offer
        //[HttpGet("Index")]
        /*[NonAction]
         public async Task<IActionResult> Index()
         {
             User user = await _userManager.GetUserAsync(User);

             if (user == null) { return NotFound(); }

             if (await _userManager.IsInRoleAsync(user, "Admin"))
             {
                 return View(await _context.Offers
                     .Include(o => o.Accommodation).ToListAsync());
             }
             else
             {
                 return View(await _context.Offers
                     .Include(o => o.Accommodation)
                     .Where(o => o.Accommodation.UserId == user.Id).ToListAsync());
             }
         }*/

        // GET: Offer/Details
        /// <summary>
        /// Method returns details about offer from database
        /// </summary>
        /// <returns> details about offer from database</returns>
        [HttpGet("details/{id:guid}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Accommodation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return (IActionResult)offer;
        }

        // GET: Offer/Create
        /*[HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            User user = await _userManager.GetUserAsync(User);

            if (user == null) { return NotFound(); }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                ViewData["AccommodationId"] = new SelectList(_context.Accommodations, "Id", "Name");
            }
            else
            {
                ViewData["AccommodationId"] = new SelectList(_context.Accommodations.Where(a => a.UserId == user.Id), "Id", "Name");
            }

            return View();
        }*/

        // POST: Offer/Create
        /// <summary>
        /// Method creates offer and adds it to database
        /// </summary>
        /// <returns>created offer from database</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("AccommodationId, StartAvailability, EndAvailability, PricePerNight, CleaningFee")] Offer offer)
        {
            /*if (ModelState.IsValid)
            {}*/
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
           // ViewData["AccommodationId"] = new SelectList(_context.Accommodations, "Id", "Id", offer.AccommodationId);
            return (IActionResult)offer;
        }

        // GET: Offer/Edit
        /*HttpGet("{id:guid}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) { return NotFound(); }

            var offer = await _context.Offers.FindAsync(id);

            if (offer == null) { return NotFound(); }

            return (IActionResult)offer;
        }*/

        // POST: Offer/Edit
        /// <summary>
        /// Method updates offer in database
        /// </summary>
        [HttpPut("{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id,
            [Bind("Id, AccommodationId, StartAvailability, EndAvailability, PricePerNight, CleaningFee")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Get offer's adding date
                offer.AddingDateTime = await _context.Offers.Where(o => o.Id == id).Select(o => o.AddingDateTime).SingleOrDefaultAsync();

                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AccommodationId"] = new SelectList(_context.Accommodations, "Id", "Id", offer.AccommodationId);
            return (IActionResult)offer;
        }

        // GET: Offer/Delete
        /*[HttpGet("Delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Accommodation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return (IActionResult)offer;
        }*/

        // POST: Offer/Delete
        /// <summary>
        /// Method deletes offer
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var offer = await _context.Offers.FindAsync(id);
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(Guid id)
        {
            return _context.Offers.Any(e => e.Id == id);
        }

        // GET: Offer/View
        /// <summary>
        /// Method returns view of offer
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> View(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Accommodation)
                .Include(o => o.Accommodation.Address)
                .Include(o => o.Accommodation.HouseRules)
                .Include(o => o.Accommodation.Pictures)
                .Include(o => o.Accommodation.Rooms)
                .ThenInclude(r => r.Amenities)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (offer == null)
            {
                NotFound($"Unable to load offer with ID '{id}'.");
            }

            var userId = _userManager.GetUserId(User);

            /*if (userId != null && BookmarkExist(offer.Id, userId))
            {
                //ViewBag.Bookmark = true;
            }
            else if (userId != null)
            {
                ViewBag.Bookmark = false;
            }

            if (TempData["AlertType"] != null && TempData["AlertMsg"] != null)
            {
                ViewBag.AlertType = TempData["AlertType"];
                ViewBag.AlertMsg = TempData["AlertMsg"];
            }*/

            return (IActionResult)offer;
        }
        /// <summary>
        /// Method adds bookmark
        /// </summary>
        [HttpGet("bookmark/{id:guid}")]
        public async Task<IActionResult> AddBookmark(Guid id)
        {
            await new BookmarkController(_context, _userManager.GetUserId(User)).Add(id);

            //TempData["AlertType"] = "success";
            //TempData["AlertMsg"] = "Offer successfully added to favorites ! <a href=\"/Identity/Account/Manage/Bookmark\">Favorits</a>";

            return RedirectToAction("View", new { id });
        }
        /// <summary>
        /// Method deletes bookmark
        /// </summary>
        [HttpDelete("bookmark/{id:guid}")]
        public async Task<IActionResult> DeleteBookmark(Guid id)
        {
            await new BookmarkController(_context, _userManager.GetUserId(User)).Delete(id);

            //TempData["AlertType"] = "warning";
            //TempData["AlertMsg"] = "Deal successfully removed from favorites ! <a href=\"/Identity/Account/Manage/Bookmark\">Favorits</a>";

            return RedirectToAction("View", new { id });
        }
        /// <summary>
        /// Method checks for a bookmark
        /// </summary>
        [HttpGet("existBookmark")]
        public bool BookmarkExist(Guid offerId, string userId)
        {
            return _context.Bookmark.Any(b => b.OfferId == offerId && b.UserId == userId);
        }
    }
}
