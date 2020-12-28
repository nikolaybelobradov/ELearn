using ELearn.Common.Enums;
using ELearn.Data.Models;
using ELearn.Services.Data.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ELearn.Services.Data.Tests
{
    public class DashboardServiceTests : BaseServiceTests
    {
        private IDashboardService Service => this.ServiceProvider.GetRequiredService<IDashboardService>();

        [Fact]
        public async Task GetLastResultsShouldReturnLastResults()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };

            await this.CreateResultAsync(user.Id, Guid.NewGuid().ToString());
            await this.CreateResultAsync(user.Id, Guid.NewGuid().ToString());

            var results = await this.Service.GetLastResultsAsync(user, 2);

            Assert.Equal(2, results.Count);
            Assert.Equal(user.Id, results.First().UserId);
            Assert.Equal(user.Id, results.Last().UserId);

        }


        private async Task<Exam> CreateExamAsync()
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
            };
            var exam = new Exam
            {
                Name = "exam",
                Description = "This is a test exam description.",
                QuestionsCount = 5,
                QuestionsOrder = OrderType.Fixed,
                ChoicesOrder = OrderType.Fixed,
                Creator = user,
                CourseId = Guid.NewGuid().ToString(),
            };

            await this.DbContext.Exams.AddAsync(exam);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<Exam>(exam).State = EntityState.Detached;

            return exam;
        }

        private async Task<Result> CreateResultAsync(string userId, string examId)
        {
            var result = new Result
            {
                UserId = userId,
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
