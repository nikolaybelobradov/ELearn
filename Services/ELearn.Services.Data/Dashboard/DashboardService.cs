namespace ELearn.Services.Data.Dashboard
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Courses;
    using Microsoft.EntityFrameworkCore;

    public class DashboardService : IDashboardService
    {
        private readonly IDeletableEntityRepository<Exam> examRepository;
        private readonly IDeletableEntityRepository<Result> resultRepository;
        private readonly IDeletableEntityRepository<Course> courseRepository;

        public DashboardService(
            IDeletableEntityRepository<Exam> examRepository,
            IDeletableEntityRepository<Result> resultRepository,
            IDeletableEntityRepository<Course> courseRepository)
        {
            this.examRepository = examRepository;
            this.resultRepository = resultRepository;
            this.courseRepository = courseRepository;
        }

        public async Task<ICollection<Result>> GetLastResultsAsync(ApplicationUser user, int count)
        {
            var results = await this.resultRepository
                .All()
                .Where(x => x.Exam.CreatorId == user.Id)
                .OrderByDescending(x => x.CreatedOn)
                .Take(count)
                .ToListAsync();

            return results;
        }

        public async Task<ICollection<Result>> GetUserLastResultsAsync(ApplicationUser user, int count)
        {
            var results = await this.resultRepository
                .All()
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.CreatedOn)
                .Take(count)
                .ToListAsync();

            return results;
        }

        public async Task<ICollection<Exam>> GetLastExamsAsync(ApplicationUser user, int count)
        {
            var exams = await this.examRepository
                .All()
                .Where(x => x.Course.Users.Contains(user))
                .OrderByDescending(x => x.CreatedOn)
                .Take(count)
                .ToListAsync();

            return exams;
        }

        public async Task<ICollection<CourseViewModel>> GetLastCoursesAsync(ApplicationUser user, int count)
        {
            var courses = await this.courseRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .Take(count)
                .To<CourseViewModel>()
                .ToListAsync();

            return courses;
        }
    }
}
