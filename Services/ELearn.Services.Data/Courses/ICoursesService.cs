namespace ELearn.Services.Data.Courses
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ELearn.Data.Models;

    public interface ICoursesService
    {
        Task CreateCourseAsync<TModel>(TModel model);

        Task<IEnumerable<T>> GetAllCoursesAsync<T>(int page, int countPerPage, string keyword = null);

        Task<int> GetAllCoursesCountAsync(string keyword = null);

        Task<ICollection<T>> GetMyCoursesWithoutPagesAsync<T>(ApplicationUser currentUser);

        Task<IEnumerable<T>> GetMyCoursesAsync<T>(ApplicationUser currentUser, int page, int countPerPage, string keyword = null);

        Task<int> GetMyCoursesCountAsync(ApplicationUser currentUser, string keyword = null);

        Task JoinCourseAsync(ApplicationUser currentUser, string courseId);

        Task ExitCourseAsync(ApplicationUser currentUser, string courseId);
    }
}
