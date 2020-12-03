namespace ELearn.Web.ViewModels.Questions
{
    using System.Collections.Generic;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class QuestionViewModel : IMapFrom<Question>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public bool IsActive { get; set; }

        public string ExamId { get; set; }

        public Exam Exam { get; set; }

        public ICollection<Choice> Choices { get; set; }
    }
}
