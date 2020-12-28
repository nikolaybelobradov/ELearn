namespace ELearn.Web.ViewModels.Courses
{
    using System.ComponentModel.DataAnnotations;

    public class EditCourseViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }
    }
}
