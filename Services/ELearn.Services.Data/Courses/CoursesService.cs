namespace ELearn.Services.Data.Courses
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Courses;
    using Microsoft.EntityFrameworkCore;

    public class CoursesService : ICoursesService
    {
        private readonly IDeletableEntityRepository<Course> courseRepository;
        private readonly IMapper mapper;

        public CoursesService(IDeletableEntityRepository<Course> courseRepository, IMapper mapper)
        {
            this.courseRepository = courseRepository;
            this.mapper = mapper;
        }

        public async Task CreateCourseAsync<TModel>(TModel model)
        {
            var course = this.mapper.Map<Course>(model);

            await this.courseRepository.AddAsync(course);
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllCoursesAsync<T>(int page, int countPerPage, string keyword = null)
        {
            var courses = this.courseRepository
                .All();

            if (!string.IsNullOrEmpty(keyword))
            {
                courses = courses.Where(x => x.Name.Contains(keyword));
            }

            var result = await courses
                .OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();

            return result;
        }

        public async Task<int> GetAllCoursesCountAsync(string keyword = null)
        {
            var courses = this.courseRepository
                .All();

            if (!string.IsNullOrEmpty(keyword))
            {
                courses = courses.Where(x => x.Name.Contains(keyword));
            }

            var result = await courses.ToListAsync();

            var count = result.Count();

            return count;
        }

        public async Task<IEnumerable<T>> GetMyCoursesAsync<T>(ApplicationUser currentUser, int page, int countPerPage, string keyword = null)
        {
            var courses = this.courseRepository
                .All().Where(x => x.Users.Contains(currentUser));

            if (!string.IsNullOrEmpty(keyword))
            {
                courses = courses.Where(x => x.Name.Contains(keyword));
            }

            var result = await courses
                .OrderByDescending(x => x.CreatedOn)
                .Skip(countPerPage * (page - 1))
                .Take(countPerPage)
                .To<T>()
                .ToListAsync();

            return result;
        }

        public async Task<int> GetMyCoursesCountAsync(ApplicationUser currentUser, string keyword = null)
        {
            var courses = this.courseRepository
                .All().Where(x => x.Users.Contains(currentUser));

            if (!string.IsNullOrEmpty(keyword))
            {
                courses = courses.Where(x => x.Name.Contains(keyword));
            }

            var result = await courses.ToListAsync();

            var count = result.Count();

            return count;
        }

        public async Task JoinCourseAsync(ApplicationUser currentUser, string courseId)
        {
            var course = await this.courseRepository.All().FirstOrDefaultAsync(x => x.Id == courseId);

            course.Users.Add(currentUser);

            await this.courseRepository.SaveChangesAsync();
        }

        public async Task ExitCourseAsync(ApplicationUser currentUser, string courseId)
        {
            var course = await this.courseRepository.All().FirstOrDefaultAsync(x => x.Id == courseId);

            var itemToRemove = course.Users.FirstOrDefault(x => x.Id == currentUser.Id);

            if (course.Users.Contains(itemToRemove))
            {
                course.Users.Remove(itemToRemove);
            }

            await this.courseRepository.SaveChangesAsync();
        }

        public async Task<ICollection<T>> GetMyCoursesWithoutPagesAsync<T>(ApplicationUser currentUser)
        {
            var courses = await this.courseRepository
                .All()
                .Where(x => x.Users.Contains(currentUser))
                .OrderByDescending(x => x.Name)
                .To<T>()
                .ToListAsync();

            return courses;
        }

        public async Task<CourseViewModel> GetCourseByIdAsync(string courseId)
        {
            var course = await this.courseRepository.All()
                .To<CourseViewModel>()
                .FirstOrDefaultAsync(x => x.Id == courseId);

            return course;
        }
    }
}
