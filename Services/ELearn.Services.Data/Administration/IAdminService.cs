namespace ELearn.Services.Data.Administration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Courses;

    public interface IAdminService
    {
        Task<ICollection<Result>> GetLastResultsAsync(ApplicationUser user);

        Task<ICollection<Exam>> GetLastExamsAsync(ApplicationUser user);

        Task<ICollection<CourseViewModel>> GetLastCoursesAsync(ApplicationUser user);
    }
}
