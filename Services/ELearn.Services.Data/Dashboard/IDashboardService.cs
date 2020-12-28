namespace ELearn.Services.Data.Dashboard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Courses;

    public interface IDashboardService
    {
        Task<ICollection<Result>> GetLastResultsAsync(ApplicationUser user, int count);

        Task<ICollection<Result>> GetUserLastResultsAsync(ApplicationUser user, int count);

        Task<ICollection<Exam>> GetLastExamsAsync(ApplicationUser user, int count);

        Task<ICollection<CourseViewModel>> GetLastCoursesAsync(int count);
    }
}
