namespace ELearn.Web.ViewModels.Questions
{
    using System.ComponentModel.DataAnnotations;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class CreateQuestionViewModel : IMapFrom<Question>
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string ExamId { get; set; }

    }
}
