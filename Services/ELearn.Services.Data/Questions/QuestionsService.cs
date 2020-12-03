namespace ELearn.Services.Data.Questions
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Questions;
    using Microsoft.EntityFrameworkCore;

    public class QuestionsService : IQuestionsService
    {
        private readonly IDeletableEntityRepository<Question> questionRepository;
        private readonly IMapper mapper;

        public QuestionsService(IDeletableEntityRepository<Question> questionRepository, IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.mapper = mapper;
        }

        public async Task CreateQuestionAsync<TModel>(TModel model)
        {
            var question = this.mapper.Map<Question>(model);

            await this.questionRepository.AddAsync(question);
            await this.questionRepository.SaveChangesAsync();
        }

        public async Task EditQuestionAsync(EditQuestionViewModel model)
        {
            var question = await this.questionRepository.All()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            question.Text = model.Text;
            question.IsActive = model.IsActive;

            this.questionRepository.Update(question);
            await this.questionRepository.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(string questionId)
        {
            var question = await this.questionRepository.All()
                .FirstOrDefaultAsync(x => x.Id == questionId);

            this.questionRepository.Delete(question);
            await this.questionRepository.SaveChangesAsync();
        }

        public async Task<QuestionViewModel> GetQuestionByIdAsync(string questionId)
        {
            var question = await this.questionRepository.All()
                .To<QuestionViewModel>()
                .FirstOrDefaultAsync(x => x.Id == questionId);

            return question;
        }
    }
}
