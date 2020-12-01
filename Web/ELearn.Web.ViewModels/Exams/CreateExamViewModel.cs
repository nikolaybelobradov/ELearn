namespace ELearn.Web.ViewModels.Exams
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ELearn.Common.Enums;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Courses;

    public class CreateExamViewModel : IMapFrom<Exam>
    {
        public CreateExamViewModel()
        {
            this.UserCourses = new HashSet<CourseViewModel>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        [Required]
        public int QuestionsCount { get; set; }

        [Required]
        public OrderType QuestionsOrder { get; set; }

        [Required]
        public OrderType ChoicesOrder { get; set; }

        public ApplicationUser Creator { get; set; }

        [Required]
        public string CourseId { get; set; }

        public ICollection<CourseViewModel> UserCourses { get; set; }
    }
}
