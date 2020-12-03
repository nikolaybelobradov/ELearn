namespace ELearn.Services
{
    using AutoMapper;
    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Choices;
    using ELearn.Web.ViewModels.Courses;
    using ELearn.Web.ViewModels.Exams;
    using ELearn.Web.ViewModels.Questions;

    public class ELearnProfile : Profile
    {
        public ELearnProfile()
        {
            // Create Course
            this.CreateMap<CreateCourseViewModel, Course>();

            // Create Exam
            this.CreateMap<CreateExamViewModel, Exam>();

            // Create Question
            this.CreateMap<CreateQuestionViewModel, Question>();

            // Add Choice
            this.CreateMap<AddChoiceViewModel, Choice>();
        }
    }
}
