namespace ELearn.Web.ViewModels.Courses
{
    using System.Collections.Generic;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class CourseViewModel : IMapFrom<Course>
    {
        public CourseViewModel()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
