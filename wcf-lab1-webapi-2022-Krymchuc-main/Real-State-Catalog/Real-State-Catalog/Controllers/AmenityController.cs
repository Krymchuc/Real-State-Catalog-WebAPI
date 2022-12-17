using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Real_State_Catalog.Models;
using Real_State_Catalog.Data;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.EntityFrameworkCore;

namespace Real_State_Catalog.Controllers
{
    [Route("Accommodation")]
    public class AmenityController : Controller
    {
        private readonly AppContextDB _context;

        public AmenityController(AppContextDB context)
        {
            _context = context;
        }

        // GET: Accommodation/ManageAmenities
        [Route("ManageAmenities/{roomId:guid?}")]
        public async Task<IActionResult> ManageAmenities(Guid? roomId)
        {
            if (roomId == null) { return NotFound(); }

            var room = await _context.Rooms
                .Include(r => r.Amenities)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null) { return NotFound(); }

            if (TempData["AlertType"] != null && TempData["AlertMsg"] != null)
            {
                ViewBag.AlertType = TempData["AlertType"];
                ViewBag.AlertMsg = TempData["AlertMsg"];
            }

            string roomType = room.RoomType.ToString();

            ViewBag.AmenityTypes = AmenityTools.AmenitiesForRoom(roomType);

            return View(room);
        }

        // POST: Accommodation/AddAmenity
        [HttpPost]
        [Route("AddAmenity/{roomId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAmenity(Guid roomId, string amenityType)
        {
            var room = await _context.Rooms
                .Include(r => r.Amenities)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null) { return NotFound(); }

            List<string> amenityTypes = AmenityTools.AmenitiesForRoom(room.RoomType.ToString());

            if (amenityTypes.Contains(amenityType))
            {
                room.Amenities.Add(new Amenity { AmenityType = (AmenityTypes)Enum.Parse(typeof(AmenityTypes), amenityType, true) });
                _context.Update(room);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["AlertType"] = "warning";
                TempData["AlertMsg"] = "Invalid equipment !";
            }

            return RedirectToAction("ManageAmenities", new { roomId });
        }

        // POST: Accommodation/DeleteAmenity
        [HttpPost]
        [Route("DeleteAmenity/{amenityId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAmenity(Guid amenityId, Guid roomId)
        {
            var nbAmenities = await _context.Amenity.CountAsync(r => r.RoomId == roomId);

            if (nbAmenities > 1)
            {
                var amenity = await _context.Amenity.FindAsync(amenityId);

                _context.Amenity.Remove(amenity);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["AlertType"] = "warning";
                TempData["AlertMsg"] = "A room must contain at least 1 piece of equipment !";
            }

            return RedirectToAction("ManageAmenities", new { roomId });
        }
    }
}
