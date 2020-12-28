using ELearn.Common.Enums;
using ELearn.Data.Models;
using ELearn.Services.Data.Dashboard;
using ELearn.Services.Mapping;
using ELearn.Web.ViewModels.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ELearn.Services.Data.Tests
{
    public class DashboardServiceTests : BaseServiceTests
    {
        private IDashboardService Service => this.ServiceProvider.GetRequiredService<IDashboardService>();

        [Fact]
        public async Task GetLastExamsShouldReturnLastExams()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };
            await this.CreateExamAsync(user);

            var exams = await this.Service.GetLastExamsAsync(user, 1);

            Assert.Equal(1, exams.Count);
            Assert.Equal("exam", exams.First().Name);
        }

        [Fact]
        public async Task GetLastUserResultsShouldReturnLastUserResults()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };

            await this.CreateResultAsync(user.Id, Guid.NewGuid().ToString());

            var results = await this.Service.GetLastResultsAsync(user, 1);

            Assert.Equal(1, results.Count);
            Assert.Equal(user.Id, results.First().UserId);
        }

        [Fact]
        public async Task GetLastResultsShouldReturnLastResults()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };
            await this.CreateResultAsync(user.Id, Guid.NewGuid().ToString());

            var results = await this.Service.GetLastResultsAsync(user, 1);

            Assert.Equal(1, results.Count);
            Assert.Equal(user.Id, results.First().UserId);
        }

        [Fact]
        public async Task GetLastCoursesAsyncShouldReturnLastCourses()
        {
            var course = await this.CreateCourseAsync();

            AutoMapperConfig.RegisterMappings(typeof(CourseViewModel).GetTypeInfo().Assembly);
            var courses = await this.Service.GetLastCoursesAsync(1);

            Assert.Single(courses);
            Assert.Equal(course.Name, courses.First().Name);
            Assert.Equal(course.Description, courses.First().Description);
        }

        private async Task<Exam> CreateExamAsync(ApplicationUser user)
        {
            var course = new Course
            {
                Id = Guid.NewGuid().ToString(),
                Name = "course",
                Description = "description",
            };
            course.Users.Add(user);

            var exam = new Exam
            {
                Name = "exam",
                Description = "This is a test exam description.",
                QuestionsCount = 5,
                QuestionsOrder = OrderType.Fixed,
                ChoicesOrder = OrderType.Fixed,
                Creator = user,
                Course = course,
                CourseId = course.Id,
            };

            await this.DbContext.Exams.AddAsync(exam);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<Exam>(exam).State = EntityState.Detached;

            return exam;
        }

        private async Task<Result> CreateResultAsync(string userId, string examId)
        {
            var exam = new Exam
            {
                Id = examId,
                CreatorId = userId,
            };

            var result = new Result
            {
                UserId = userId,
                Exam = exam,
                ExamId = examId,
                Points = 100,
            };

            await this.DbContext.Results.AddAsync(result);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<Result>(result).State = EntityState.Detached;

            return result;
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
