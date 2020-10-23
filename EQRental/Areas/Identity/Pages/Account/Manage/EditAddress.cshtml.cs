using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EQRental.Data;
using EQRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EQRental.Areas.Identity.Pages.Account.Manage
{
    public class EditAddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext context;

        public EditAddressModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            this.context = context;
        }

        [BindProperty]
        public UserAddress Address { get; set; }

        private async Task LoadAsync(ApplicationUser user, int id)
        {
            var address = await (from a in context.UserAddresses
                                 where a.UserID == user.Id && a.ID == id
                                 select a).SingleOrDefaultAsync();
            Address = address;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id == null || user == null)
            {
                return NotFound();
            }
            await LoadAsync(user, (int)id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user, Address.ID);
                return Page();
            }

            var address = await (from a in context.UserAddresses
                                 where a.ID == Address.ID
                                 select a).SingleOrDefaultAsync();

            if (address == null)
            {
                return NotFound();
            }

            address.PostalCode = Address.PostalCode;
            address.City = Address.City;
            address.Street = Address.Street;

            await context.SaveChangesAsync();
            return RedirectToPage("AllAddress");
        }
    }
}
