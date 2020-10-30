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
    public class AllAddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;

        public AllAddressModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public List<UserAddress> Addresses { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            Addresses = await (from a in context.UserAddresses
                                   where a.UserID == user.Id
                                   select a).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnGetDelete(int? id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (id != null)
            {
                var address = await (from a in context.UserAddresses
                            where a.ID == id && a.UserID == user.Id
                            select a).SingleOrDefaultAsync();

                if (address == null)
                {
                    return NotFound($"Unable to find address with ID '{id}'.");
                }

                context.Remove(address);
                await context.SaveChangesAsync();
            }

            return RedirectToPage("AllAddress");
        }
    }
}
