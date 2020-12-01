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
        public async Task<IActionResult> All(int page = 1, int countPerPage = PerPageDefaultValue, string keyword = null)
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
    }
}
