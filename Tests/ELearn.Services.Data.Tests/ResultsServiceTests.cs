using ELearn.Data.Models;
using ELearn.Services.Data.Results;
using ELearn.Services.Mapping;
using ELearn.Web.ViewModels.Results;
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
    public class ResultsServiceTests : BaseServiceTests
    {
        private IResultsService Service => this.ServiceProvider.GetRequiredService<IResultsService>();

        [Fact]
        public async Task GetUserResultByExamIdShouldReturnUserResultByExamId()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };
            var examId = Guid.NewGuid().ToString();

            var result = await this.CreateResultAsync(user.Id, examId);

            AutoMapperConfig.RegisterMappings(typeof(ResultViewModel).GetTypeInfo().Assembly);
            var userResultPoints = await this.Service.GetUserResultByExamIdAsync(examId, user.Id);

            Assert.Equal(result.Points, userResultPoints);
        }

        [Fact]
        public async Task GetUserResultsShouldReturnUserResults()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };

            var result = await this.CreateResultAsync(user.Id, Guid.NewGuid().ToString());
            var result2 = await this.CreateResultAsync(user.Id, Guid.NewGuid().ToString());
            var editedQuestion = await this.DbContext.Results.FirstOrDefaultAsync(x => x.Id == result.Id);
            var editedQuestion2 = await this.DbContext.Results.FirstOrDefaultAsync(x => x.Id == result2.Id);
            AutoMapperConfig.RegisterMappings(typeof(ResultViewModel).GetTypeInfo().Assembly);
            var results = await this.Service.GetUserResultsAsync<ResultViewModel>(user, 1, 10);
            var asd = 1;
            Assert.Equal(2, results.Count());
            Assert.Equal(result2.UserId, results.First().User.Id);
            Assert.Equal(result2.ExamId, results.First().Exam.Id);
            Assert.Equal(result2.Points, results.First().Points);
        }

        [Fact]
        public async Task GetUserResultsCountShouldReturnCorrectCountOfUserResults()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };

            await this.CreateResultAsync(user.Id, Guid.NewGuid().ToString());

            var count = await this.Service.GetUserResultsCountAsync(user);

            Assert.Equal(1, count);
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
    }
}
