namespace ELearn.Services.Data.Exams
{
    using System.Threading.Tasks;

    using AutoMapper;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Exams;
    using Microsoft.EntityFrameworkCore;

    public class ExamsService : IExamsService
    {
        private readonly IDeletableEntityRepository<Exam> examRepository;
        private readonly IMapper mapper;

        public ExamsService(IDeletableEntityRepository<Exam> examRepository, IMapper mapper)
        {
            this.examRepository = examRepository;
            this.mapper = mapper;
        }

        public async Task CreateExamAsync<TModel>(TModel model)
        {
            var exam = this.mapper.Map<Exam>(model);

            await this.examRepository.AddAsync(exam);
            await this.examRepository.SaveChangesAsync();
        }

        public async Task<ExamViewModel> GetExamByIdAsync(string examId)
        {
            var exam = await this.examRepository.All()
                .To<ExamViewModel>()
                .FirstOrDefaultAsync(x => x.Id == examId);

            return exam;
        }
    }
}
