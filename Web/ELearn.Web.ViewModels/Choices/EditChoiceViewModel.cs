namespace ELearn.Web.ViewModels.Choices
{
    using System.ComponentModel.DataAnnotations;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class EditChoiceViewModel : IMapFrom<Choice>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsTrue { get; set; }
    }
}
