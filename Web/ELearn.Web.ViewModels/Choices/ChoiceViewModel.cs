namespace ELearn.Web.ViewModels.Choices
{
    using System;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class ChoiceViewModel : IMapFrom<Choice>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public bool IsTrue { get; set; }

        public bool IsSelected { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
