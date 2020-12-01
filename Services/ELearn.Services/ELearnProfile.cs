namespace ELearn.Services
{
    using AutoMapper;
    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Courses;
    using ELearn.Web.ViewModels.Exams;

    public class ELearnProfile : Profile
    {
        public ELearnProfile()
        {
            // Create Course
            this.CreateMap<CreateCourseViewModel, Course>();

            this.CreateMap<Course, Course>();

            // Create Exam

            this.CreateMap<CreateExamViewModel, Exam>();
        }
    }
}
