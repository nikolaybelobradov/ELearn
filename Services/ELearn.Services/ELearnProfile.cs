namespace ELearn.Services
{
    using AutoMapper;
    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Courses;

    public class ELearnProfile : Profile
    {
        public ELearnProfile()
        {
            // Create Course
            this.CreateMap<CreateCourseViewModel, Course>();
        }
    }
}
