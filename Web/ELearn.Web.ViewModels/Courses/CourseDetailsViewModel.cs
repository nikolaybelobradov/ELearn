namespace ELearn.Web.ViewModels.Courses
{
    using System.Collections.Generic;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Exams;

    public class CourseDetailsViewModel : IMapFrom<Course>
    {
        public CourseDetailsViewModel()
        {
            this.Exams = new HashSet<ExamViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<ExamViewModel> Exams { get; set; }

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
