namespace ELearn.Web.ViewModels.Courses
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class CreateCourseViewModel : IMapFrom<Course>
    {
        public CreateCourseViewModel()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
