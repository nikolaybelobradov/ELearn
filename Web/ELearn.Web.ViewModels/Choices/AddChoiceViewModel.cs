namespace ELearn.Web.ViewModels.Choices
{
    using System.ComponentModel.DataAnnotations;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class AddChoiceViewModel : IMapFrom<Choice>
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsTrue { get; set; }

        public string QuestionId { get; set; }
    }
}
