using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ELearn.Data.Models;
using ELearn.Services.Data.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ELearn.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUsersService usersService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUsersService usersService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.usersService = usersService;

        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Middle Name")]
            public string MiddleName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var firstName = await usersService.GetFirstNameAsync(user);
            var middleName = await usersService.GetMiddleNameAsync(user);
            var lastName = await usersService.GetLastNameAsync(user);

            this.Username = userName;

            this.Input = new InputModel
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
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

            var firstName = await this.usersService.GetFirstNameAsync(user);
            if (this.Input.FirstName != firstName)
            {
                var setFirstNameResult = await this.usersService.SetFirstNameAsync(user, this.Input.FirstName);
                if (!setFirstNameResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set first name.";
                    return this.RedirectToPage();
                }
            }

            var middleName = await this.usersService.GetMiddleNameAsync(user);
            if (this.Input.MiddleName != middleName)
            {
                var setMiddleNameResult = await this.usersService.SetMiddleNameAsync(user, this.Input.MiddleName);
                if (!setMiddleNameResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set middle name.";
                    return this.RedirectToPage();
                }
            }

            var lastName = await this.usersService.GetLastNameAsync(user);
            if (this.Input.LastName != lastName)
            {
                var setLastNameResult = await this.usersService.SetLastNameAsync(user, this.Input.LastName);
                if (!setLastNameResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set last name.";
                    return this.RedirectToPage();
                }
            }

            await this._signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }
    }
}
