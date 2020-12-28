namespace ELearn.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Services.Data.Courses;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Courses;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class CoursesServiceTests : BaseServiceTests
    {
        private ICoursesService Service => this.ServiceProvider.GetRequiredService<ICoursesService>();

        [Fact]
        public async Task GetCourseByIdAsyncShouldWorkCorrectly()
        {
            var course = await this.CreateCourseAsync();

            AutoMapperConfig.RegisterMappings(typeof(CourseViewModel).GetTypeInfo().Assembly);
            var result = await this.Service.GetCourseByIdAsync(course.Id);

            Assert.Equal(course.Id, result.Id);
            Assert.Equal(course.Name, result.Name);
            Assert.Equal(course.Description, result.Description);
        }

        [Fact]
        public async Task CreateCourseAsyncShouldCreateNewCourse()
        {
            var createCourseViewModel = new CreateCourseViewModel
            {
                Name = "course",
                Description = "This is a test course description.",
            };

            await this.Service.CreateCourseAsync(createCourseViewModel);

            var course = await this.DbContext.Courses.FirstOrDefaultAsync();
            var coursesCount = this.DbContext.Courses.ToArray().Count();

            Assert.Equal(1, coursesCount);
            Assert.Equal("course", course.Name);
            Assert.Equal("This is a test course description.", course.Description);
        }

        [Fact]
        public async Task EditCourseAsyncShouldEditCorrectly()
        {
            var newName = "New Course Name";
            var newDescription = "This is a new test course description.";

            var course = await this.CreateCourseAsync();

            var editCourseViewModel = new EditCourseViewModel
            {
                Id = course.Id,
                Name = newName,
                Description = newDescription,
            };

            await this.Service.EditCourseAsync(editCourseViewModel);
            var editedCourse = await this.DbContext.Courses.FirstOrDefaultAsync(x => x.Id == course.Id);

            Assert.Equal(newName, editedCourse.Name);
            Assert.Equal(newDescription, editedCourse.Description);
        }

        [Fact]
        public async Task GetAllCoursesAsyncShouldReturnAllCourses()
        {
            var course = await this.CreateCourseAsync();

            AutoMapperConfig.RegisterMappings(typeof(CourseViewModel).GetTypeInfo().Assembly);
            var courses = await this.Service.GetAllCoursesAsync<CourseViewModel>(1, 10);

            Assert.Single(courses);
            Assert.Equal(course.Name, courses.First().Name);
            Assert.Equal(course.Description, courses.First().Description);
        }

        [Fact]
        public async Task GetMyCoursesAsyncShouldReturnMyCourses()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };
            var createCourseViewModel = new CreateCourseViewModel
            {
                Name = "course",
                Description = "This is a test course description.",
            };

            createCourseViewModel.Users.Add(user);

            await this.Service.CreateCourseAsync(createCourseViewModel);

            AutoMapperConfig.RegisterMappings(typeof(CourseViewModel).GetTypeInfo().Assembly);
            var courses = await this.Service.GetMyCoursesAsync<CourseViewModel>(user, 1, 10);

            Assert.Single(courses);
            Assert.Equal(createCourseViewModel.Name, courses.First().Name);
            Assert.Equal(createCourseViewModel.Description, courses.First().Description);
        }

        [Fact]
        public async Task GetMyCoursesWithoutPagesAsyncShouldReturnMyCourses()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };
            var createCourseViewModel = new CreateCourseViewModel
            {
                Name = "course",
                Description = "This is a test course description.",
            };

            createCourseViewModel.Users.Add(user);

            await this.Service.CreateCourseAsync(createCourseViewModel);

            AutoMapperConfig.RegisterMappings(typeof(CourseViewModel).GetTypeInfo().Assembly);
            var courses = await this.Service.GetMyCoursesWithoutPagesAsync<CourseViewModel>(user);

            Assert.Single(courses);
            Assert.Equal(createCourseViewModel.Name, courses.First().Name);
            Assert.Equal(createCourseViewModel.Description, courses.First().Description);
        }

        [Fact]
        public async Task GetAllCoursesCountAsyncShouldReturnCorrectCountOfAllCourses()
        {
            await this.CreateCourseAsync();

            var count = await this.Service.GetAllCoursesCountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetMyCoursesCountAsyncShouldReturnCorrectCountOfMyCourses()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };
            var createCourseViewModel = new CreateCourseViewModel
            {
                Name = "course",
                Description = "This is a test course description.",
            };

            createCourseViewModel.Users.Add(user);

            await this.Service.CreateCourseAsync(createCourseViewModel);

            var count = await this.Service.GetMyCoursesCountAsync(user);

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task DeleteCourseAsyncShouldDeleteCorrectly()
        {
            var course = await this.CreateCourseAsync();

            await this.Service.DeleteCourseAsync(course.Id);

            var coursesCount = this.DbContext.Courses.Where(x => !x.IsDeleted).ToArray().Count();
            var deletedCourse = await this.DbContext.Courses.FirstOrDefaultAsync(x => x.Id == course.Id);

            Assert.Equal(0, coursesCount);
            Assert.Null(deletedCourse);
        }

        [Fact]

        public async Task JoinCourseAsyncShouldWorkCorrectly()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };
            var createCourseViewModel = new CreateCourseViewModel
            {
                Name = "course",
                Description = "This is a test course description.",
            };

            await this.Service.CreateCourseAsync(createCourseViewModel);

            var course = await this.DbContext.Courses.FirstOrDefaultAsync();

            await this.Service.JoinCourseAsync(user, course.Id);

            Assert.Contains(user, course.Users);
        }

        [Fact]

        public async Task RemoveUserFromCourseAsyncShouldWorkCorrectly()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };
            var createCourseViewModel = new CreateCourseViewModel
            {
                Name = "course",
                Description = "This is a test course description.",
            };

            createCourseViewModel.Users.Add(user);

            await this.Service.CreateCourseAsync(createCourseViewModel);

            var course = await this.DbContext.Courses.FirstOrDefaultAsync();

            Assert.Contains(user, course.Users);

            await this.Service.RemoveUserFromCourseAsync(course.Id, user.Id);

            Assert.DoesNotContain(user, course.Users);
        }

        private async Task<Course> CreateCourseAsync()
        {
            var course = new Course
            {
                Name = "course",
                Description = "This is a test course description.",
            };

            await this.DbContext.Courses.AddAsync(course);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<Course>(course).State = EntityState.Detached;

            return course;
        }
    }
}
