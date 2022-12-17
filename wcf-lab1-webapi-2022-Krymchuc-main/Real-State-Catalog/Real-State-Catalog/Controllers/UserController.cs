using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Real_State_Catalog.Models;
using System.ComponentModel.DataAnnotations;
using Real_State_Catalog.Data;

namespace Real_State_Catalog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController:Controller
    {
        private readonly AppContextDB _context;
        private readonly UserManager<User> _userManager;

        public UserController(AppContextDB context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string UserId;

        public string UserEmail { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "You must enter a first name")]
            [Display(Name = "First name")]
            public string? FirstName { get; set; }

            [Required(ErrorMessage = "You must enter a name")]
            [Display(Name = "Last name")]
            public string? LastName { get; set; }

            [Required]
            [RegularExpression("User|Host|Admin", ErrorMessage = "Please select a valid role")]
            [Display(Name = "Role")]
            public string? Role { get; set; }
        }

        // GET: UserController
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        // GET: UserController/Details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Accommodations)
                .ThenInclude(a => a.Address)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{id}'");
            }

            return View(user);
        }

        // GET: UserController/Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound($"Id not specified");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{id}'");
            }

            UserEmail = user.Email;
            UserId = user.Id;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
            };

            return View(this);
        }

        // POST: UserController/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(String userId, IFormCollection collection)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserId}'");
            }

            if (ModelState.IsValid)
            {
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;

                string actualRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                if (actualRole == null)
                {
                    // Встановити роль користувача
                    await _userManager.AddToRoleAsync(user, Input.Role);
                }
                else if (!actualRole.Equals(Input.Role))
                {
                    // У користувача вже є роль, тому спочатку видаліть фактичну роль
                    await _userManager.RemoveFromRoleAsync(user, actualRole);

                    // Потім встановіть нову роль
                    await _userManager.AddToRoleAsync(user, Input.Role);
                }

                
                await _userManager.UpdateSecurityStampAsync(user);
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound($"Id not specified");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{id}'");
            }

            return View(user);
        }

        // POST: UserController/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound($"Id not specified");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{id}'");
            }

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
