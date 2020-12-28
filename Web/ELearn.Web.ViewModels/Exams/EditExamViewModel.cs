namespace ELearn.Web.ViewModels.Exams
{
    using System.ComponentModel.DataAnnotations;

    using ELearn.Common.Enums;

    public class EditExamViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        [Required]
        public int QuestionsCount { get; set; }

        [Required]
        public OrderType QuestionsOrder { get; set; }

        [Required]
        public OrderType ChoicesOrder { get; set; }
    }
}
