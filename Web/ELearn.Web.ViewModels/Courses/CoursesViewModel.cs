namespace ELearn.Web.ViewModels.Courses
{
    using System.Collections.Generic;

    public class CoursesViewModel
    {
        public CoursesViewModel()
        {
            this.Courses = new HashSet<CourseViewModel>();
        }

        public IEnumerable<CourseViewModel> Courses { get; set; }

        public string Keyword { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public int NextPage
        {
            get
            {
                if (this.CurrentPage >= this.PagesCount)
                {
                    return 1;
                }

                return this.CurrentPage + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.CurrentPage <= 1)
                {
                    return this.PagesCount;
                }

                return this.CurrentPage - 1;
            }
        }
    }
}