namespace ELearn.Services.Data.Results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ResultsService : IResultsService
    {
        private readonly IDeletableEntityRepository<Result> resultRepository;

        public ResultsService(IDeletableEntityRepository<Result> resultRepository)
        {
            this.resultRepository = resultRepository;
        }

        public async Task<int> GetUserResultByExamIdAsync(string examId, string userId)
        {
            var result = await this.resultRepository.All()
                .FirstOrDefaultAsync(x => (x.ExamId == examId) && (x.UserId == userId));

            return result.Points;
        }

        public async Task<IEnumerable<T>> GetUserResultsAsync<T>(ApplicationUser user, int page, int countPerPage, string keyword = null)
        {
            var results = this.resultRepository
                .All()
                .Where(x => x.UserId == user.Id);

            if (!string.IsNullOrEmpty(keyword))
            {
                results = results.Where(x => x.Exam.Name.Contains(keyword));
            }

            var result = await results
                .OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();

            return result;
        }

        public async Task<int> GetUserResultsCountAsync(ApplicationUser user, string keyword = null)
        {
            var results = this.resultRepository
                .All()
                .Where(x => x.UserId == user.Id);

            if (!string.IsNullOrEmpty(keyword))
            {
                results = results.Where(x => x.Exam.Name.Contains(keyword));
            }

            var result = await results.ToListAsync();

            var count = result.Count();

            return count;
        }
    }
}
