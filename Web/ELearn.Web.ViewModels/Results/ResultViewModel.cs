namespace ELearn.Web.ViewModels.Results
{
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class ResultViewModel : IMapFrom<Result>
    {
        public int Points { get; set; }

        public ApplicationUser User { get; set; }

        public Exam Exam { get; set; }
    }
}
