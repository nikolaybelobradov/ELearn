namespace ELearn.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Courses;

    public class HomePageViewModel
    {
        public HomePageViewModel()
        {
            this.LastExams = new HashSet<Exam>();
            this.LastResults = new HashSet<Result>();
            this.LastCourses = new HashSet<CourseViewModel>();
        }

        public ICollection<Exam> LastExams { get; set; }

        public ICollection<Result> LastResults { get; set; }

        public ICollection<CourseViewModel> LastCourses { get; set; }
    }
}
