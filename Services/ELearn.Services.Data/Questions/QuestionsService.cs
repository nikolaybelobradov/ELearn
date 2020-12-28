namespace ELearn.Services.Data.Questions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Choices;
    using ELearn.Web.ViewModels.Questions;
    using Microsoft.EntityFrameworkCore;

    public class QuestionsService : IQuestionsService
    {
        private readonly IDeletableEntityRepository<Question> questionRepository;
        private readonly IDeletableEntityRepository<Choice> choiceRepository;
        private readonly IMapper mapper;

        public QuestionsService(IDeletableEntityRepository<Question> questionRepository, IDeletableEntityRepository<Choice> choiceRepository, IMapper mapper)
        {
            this.questionRepository = questionRepository;
            this.choiceRepository = choiceRepository;
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

            if (question == null)
            {
                throw new ArgumentException("There is no question with this id.");
            }

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
                .FirstOrDefaultAsync(x => x.Id == questionId);

            var choices = await this.choiceRepository.All()
                .Where(x => x.QuestionId == question.Id)
                .To<ChoiceViewModel>()
                .ToListAsync();

            var result = new QuestionViewModel
            {
                Id = question.Id,
                Text = question.Text,
                IsActive = question.IsActive,
                ExamId = question.ExamId,
                Exam = question.Exam,
                Choices = choices,
                CreatedOn = question.CreatedOn,
            };

            return result;
        }
    }
}
