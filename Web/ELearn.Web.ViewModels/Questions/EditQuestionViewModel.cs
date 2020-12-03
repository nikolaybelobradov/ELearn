namespace ELearn.Web.ViewModels.Questions
{
    using System.ComponentModel.DataAnnotations;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class EditQuestionViewModel : IMapFrom<Question>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
