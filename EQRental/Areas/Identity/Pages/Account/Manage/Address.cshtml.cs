using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public class AddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext context;

        public AddressModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            this.context = context;
        }

        [BindProperty]
        public UserAddress Address { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var address = Address;
            address.UserID = user.Id;

            context.UserAddresses.Add(address);

            await context.SaveChangesAsync();
            return RedirectToPage("AllAddress");
        }
    }
}
