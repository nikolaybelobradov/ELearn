namespace ELearn.Web.ViewModels.Exams
{
    using System.Collections.Generic;

    using ELearn.Common.Enums;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Questions;

    public class ExamViewModel : IMapFrom<Exam>
    {
        public ExamViewModel()
        {
            this.Questions = new List<QuestionViewModel>();
            this.Results = new HashSet<Result>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int QuestionsCount { get; set; }

        public OrderType QuestionsOrder { get; set; }

        public OrderType ChoicesOrder { get; set; }

        public Course Course { get; set; }

        public ApplicationUser Creator { get; set; }

        public IList<QuestionViewModel> Questions { get; set; }

        public ICollection<Result> Results { get; set; }
    }
}
