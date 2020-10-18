using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EQRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EQRental.Controllers;

namespace EQRental.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Full name")]
            public string FullName { get; set; }

            [Display(Name = "Bank account")]
            public string BankAccount { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                Username = userName,
                PhoneNumber = phoneNumber,
                FullName = user.FullName,
                BankAccount = user.BankAccount,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var userName = await _userManager.GetUserNameAsync(user);
            if (Input.Username != userName)
            {
                var userNameExists = await _userManager.FindByNameAsync(Input.Username);
                if (userNameExists != null)
                {
                    StatusMessage = "User name already taken. Select a different username.";
                    return RedirectToPage();
                }
                var setUserName = await _userManager.SetUserNameAsync(user, Input.Username);
                if (!setUserName.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set user name.";
                    return RedirectToPage();
                }
            }

            if (Input.FullName != user.FullName)
            {
                user.FullName = Input.FullName;
                var setFullNameResult = await _userManager.UpdateAsync(user);
                if (!setFullNameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set fullname.";
                    return RedirectToPage();
                }
            }

            if (Input.BankAccount != user.BankAccount)
            {
                user.BankAccount = Input.BankAccount;
                var setBankAccountResult = await _userManager.UpdateAsync(user);
                if (!setBankAccountResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set bank account.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
