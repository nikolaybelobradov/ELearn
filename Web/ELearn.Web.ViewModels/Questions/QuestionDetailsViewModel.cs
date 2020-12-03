namespace ELearn.Web.ViewModels.Questions
{
    using ELearn.Web.ViewModels.Choices;

    public class QuestionDetailsViewModel
    {
        public QuestionViewModel QuestionViewModel { get; set; }

        public AddChoiceViewModel AddChoiceViewModel { get; set; }
    }
}
