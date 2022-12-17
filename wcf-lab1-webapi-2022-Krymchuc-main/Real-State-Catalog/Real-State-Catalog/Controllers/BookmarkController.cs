using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Real_State_Catalog.Models;
using Real_State_Catalog.Data;

namespace Real_State_Catalog.Controllers
{
    [Authorize]
    public class BookmarkController : Controller
    {
        private readonly AppContextDB _context;
        private readonly string _userId;

        public BookmarkController(AppContextDB context, string userId)
        {
            _context = context;
            _userId = userId;
        }

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
        private Bookmark BookmarkExists(Guid offerId)
        {
            return _context.Bookmark.Where(b => b.OfferId == offerId && b.UserId == _userId).SingleOrDefault();
        }
    }
}
