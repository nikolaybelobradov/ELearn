namespace ELearn.Web.ViewModels.Courses
{
    using System.Collections.Generic;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Exams;
    using ELearn.Web.ViewModels.Users;

    public class CourseViewModel : IMapFrom<Course>
    {
        public CourseViewModel()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Exams = new HashSet<ExamViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<ExamViewModel> Exams { get; set; }
    }
}
