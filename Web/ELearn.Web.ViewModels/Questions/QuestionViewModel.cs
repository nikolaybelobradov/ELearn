namespace ELearn.Web.ViewModels.Questions
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Choices;

    public class QuestionViewModel : IMapFrom<Question>
    {
        public QuestionViewModel()
        {
            this.Choices = new List<ChoiceViewModel>();
        }

        public string Id { get; set; }

        public string Text { get; set; }

        public bool IsActive { get; set; }

        public string ExamId { get; set; }

        public Exam Exam { get; set; }

        public IList<ChoiceViewModel> Choices { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
