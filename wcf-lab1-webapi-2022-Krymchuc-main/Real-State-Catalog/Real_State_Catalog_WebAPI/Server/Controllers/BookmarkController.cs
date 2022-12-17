using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Real_State_Catalog_WebAPI.Models;
using Real_State_Catalog_WebAPI.Data;

namespace Real_State_Catalog_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookmarkController : ControllerBase
    {
        private readonly AppContextDB _context;
        private readonly string _userId;

        public BookmarkController(AppContextDB context, string userId)
        {
            _context = context;
            _userId = userId;
        }
        /// <summary>
        /// Method adds a bookmark
        /// </summary>
        [HttpPost("{offerId:guid}")]
        public async Task Add(Guid offerId)
        {
            // Перевірте, чи вже існує закладка для підключеного користувача
            if (BookmarkExists(offerId) == null)
            {
                Bookmark bookmark = new();
                bookmark.OfferId = offerId;
                bookmark.UserId = _userId;

                await _context.Bookmark.AddAsync(bookmark);
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Method deletes a bookmark
        /// </summary>
        [HttpDelete("{offerId:guid}")]
        public async Task Delete(Guid offerId)
        {
            var bookmark = BookmarkExists(offerId);

            if (bookmark != null)
            {
                _context.Bookmark.Remove(bookmark);
                await _context.SaveChangesAsync();
            }
        }

        // Перевірка, чи вже існує закладка для фактично підключеного користувача
        // Повернути закладку, якщо вона існує
        /// <summary>
        /// Method checks for a bookmark
        /// </summary>
        [HttpGet("{offerId:guid}")]
        public Bookmark BookmarkExists(Guid offerId)
        {
            return _context.Bookmark.Where(b => b.OfferId == offerId && b.UserId == _userId).SingleOrDefault();
        }
    }
}
