namespace ELearn.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ELearn.Common;
    using ELearn.Data.Models;
    using ELearn.Services.Data.Users;
    using ELearn.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class UsersController : AdministrationController
    {
        private const int PerPageDefaultValue = 10;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UsersController(
            IUsersService usersService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int countPerPage = PerPageDefaultValue, string keyword = null)
        {
            var users = await this.usersService.GetAllUsersAsync<UserViewModel>(page, countPerPage, keyword);

            foreach (var user in users)
            {
                var currentUser = await this.userManager.FindByIdAsync(user.Id);
                var roles = await this.userManager.GetRolesAsync(currentUser);

                if (roles.Count == 0)
                {
                    user.Role = "None";
                }
                else
                {
                    user.Role = string.Join(",", roles);
                }
            }

            var usersCount = await this.usersService.GetAllUsersCountAsync(keyword);
            var model = new AllUsersViewModel()
            {
                Users = users,
                CurrentPage = page,
                PagesCount = (int)Math.Ceiling(usersCount / (decimal)countPerPage),
            };
            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            var roles = await this.userManager.GetRolesAsync(user);

            var role = "None";
            if (roles.Count != 0)
            {
                role = string.Join(",", roles);
            }

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Role = role,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            await this.usersService.EditUserAsync(viewModel);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await this.usersService.DeleteUserAsync(id);

            return this.RedirectToAction("Index");
        }
    }
}
