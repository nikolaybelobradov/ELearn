namespace ELearn.Services.Data.Tests
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ELearn.Common;
    using ELearn.Data.Models;
    using ELearn.Services.Data.Users;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using NuGet.Frameworks;
    using Xunit;

    public class UsersServiceTests : BaseServiceTests
    {
        private IUsersService Service => this.ServiceProvider.GetRequiredService<IUsersService>();

        private RoleManager<ApplicationRole> RoleManager => this.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        [Theory]
        [InlineData(GlobalConstants.AdministratorRoleName)]
        [InlineData(GlobalConstants.LecturerRoleName)]
        [InlineData("None")]

        public async Task EditUserAsyncShouldEditCorrectly(string roleName)
        {
            await this.RoleManager.CreateAsync(new ApplicationRole(GlobalConstants.AdministratorRoleName));
            await this.RoleManager.CreateAsync(new ApplicationRole(GlobalConstants.LecturerRoleName));

            var newFirstName = "Georgi";
            var newMiddleName = "Ivanov";
            var newLastName = "Dimitrov";

            var user = await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);

            var editUserViewModel = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = newFirstName,
                MiddleName = newMiddleName,
                LastName = newLastName,
                Role = roleName,
            };

            await this.Service.EditUserAsync(editUserViewModel);
            var editedUser = await this.DbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            Assert.Equal(newFirstName, editedUser.FirstName);
            Assert.Equal(newMiddleName, editedUser.MiddleName);
            Assert.Equal(newLastName, editedUser.LastName);

            if (roleName == "None")
            {
                Assert.Empty(editedUser.Roles);
            }
            else
            {
                var role = await this.RoleManager.FindByIdAsync(editedUser.Roles.FirstOrDefault().RoleId);

                Assert.Equal(roleName, role.Name);
            }
        }

        [Fact]
        public async Task GetFirstNameShouldReturnFirstName()
        {
            var user = await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);

            var firstName = await this.Service.GetFirstNameAsync(user);

            Assert.Equal("Petur", firstName);
        }

        [Fact]
        public async Task GetMiddleNameShouldReturnMiddleName()
        {
            var user = await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);

            var middleName = await this.Service.GetMiddleNameAsync(user);

            Assert.Equal("Petrov", middleName);
        }

        [Fact]
        public async Task GetLastNameShouldReturnLastName()
        {
            var user = await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);

            var lastName = await this.Service.GetLastNameAsync(user);

            Assert.Equal("Petkov", lastName);
        }

        [Fact]
        public async Task SetFirstNameShouldSetFirstName()
        {
            var user = await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);

            var result = await this.Service.SetFirstNameAsync(user, "Ivan");

            var editedUser = await this.DbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            Assert.Equal("Ivan", editedUser.FirstName);
            Assert.Equal(IdentityResult.Success, result);
        }

        [Fact]
        public async Task SetMiddleNameShouldSetFirstName()
        {
            var user = await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);

            var result = await this.Service.SetMiddleNameAsync(user, "Todorov");

            var editedUser = await this.DbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            Assert.Equal("Todorov", editedUser.MiddleName);
            Assert.Equal(IdentityResult.Success, result);
        }

        [Fact]
        public async Task SetLastNameShouldSetLastName()
        {
            var user = await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);

            var result = await this.Service.SetLastNameAsync(user, "Dimitrov");

            var editedUser = await this.DbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            Assert.Equal("Dimitrov", editedUser.LastName);
            Assert.Equal(IdentityResult.Success, result);
        }

        [Fact]
        public async Task DeleteExamAsyncShouldDeleteCorrectly()
        {
            var user = await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);

            await this.Service.DeleteUserAsync(user.Id);

            var usersCount = this.DbContext.Users.Where(x => !x.IsDeleted).ToArray().Count();
            var deletedUser = await this.DbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            Assert.Equal(0, usersCount);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task GetAllUsersShouldReturnAllUsers()
        {
            await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);
            var user = await this.CreateUserAsync("mymail2@elearn.com", "Ivan", "Todorov", "Dimitrov", GlobalConstants.LecturerRoleName);

            AutoMapperConfig.RegisterMappings(typeof(UserViewModel).GetTypeInfo().Assembly);
            var users = await this.Service.GetAllUsersAsync<UserViewModel>(1, 10);

            Assert.Equal(2, users.Count());
            Assert.Equal(user.Email, users.First().Email);
            Assert.Equal(user.FirstName, users.First().FirstName);
            Assert.Equal(user.MiddleName, users.First().MiddleName);
            Assert.Equal(user.LastName, users.First().LastName);
        }

        [Fact]
        public async Task GetAllUsersCountShouldReturnCorrectCountOfAllUsers()
        {
            await this.CreateUserAsync("mymail@elearn.com", "Petur", "Petrov", "Petkov", null);

            var count = await this.Service.GetAllUsersCountAsync();

            Assert.Equal(1, count);
        }

        private async Task<ApplicationUser> CreateUserAsync(string email, string firstName, string middleName, string lastName, string roleName = null)
        {
            var user = new ApplicationUser()
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Email = email,
                UserName = email,
            };

            if (roleName != null)
            {
                var role = new ApplicationRole()
                {
                    Name = roleName,
                };

                await this.DbContext.Roles.AddAsync(role);
                var userRole = new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id,
                };

                await this.DbContext.UserRoles.AddAsync(userRole);
                await this.DbContext.SaveChangesAsync();
                this.DbContext.Entry<ApplicationRole>(role).State = EntityState.Detached;
                this.DbContext.Entry<IdentityUserRole<string>>(userRole).State = EntityState.Detached;
            }

            await this.DbContext.Users.AddAsync(user);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<ApplicationUser>(user).State = EntityState.Detached;
            return user;
        }
    }
}
