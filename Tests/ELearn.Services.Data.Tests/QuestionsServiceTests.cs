namespace ELearn.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Services.Data.Questions;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Questions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class QuestionsServiceTests : BaseServiceTests
    {
        private IQuestionsService Service => this.ServiceProvider.GetRequiredService<IQuestionsService>();

        [Fact]
        public async Task CreateQuestionAsyncShouldCreateNewQuestion()
        {
            var examId = Guid.NewGuid().ToString();
            var createQuestionViewModel = new CreateQuestionViewModel
            {
                Text = "question",
                IsActive = true,
                ExamId = examId,
            };

            await this.Service.CreateQuestionAsync(createQuestionViewModel);

            var question = await this.DbContext.Questions.FirstOrDefaultAsync();
            var questionsCount = this.DbContext.Questions.ToArray().Count();

            Assert.Equal(1, questionsCount);
            Assert.Equal("question", question.Text);
            Assert.True(question.IsActive);
            Assert.Equal(examId, question.ExamId);
        }

        [Fact]
        public async Task EditChoiceAsyncShouldEditCorrectly()
        {
            var newText = "New Question Text";
            var newIsActive = false;

            var question = await this.CreateQuestionAsync();

            var editQuestionViewModel = new EditQuestionViewModel
            {
                Id = question.Id,
                Text = newText,
                IsActive = newIsActive,
            };

            await this.Service.EditQuestionAsync(editQuestionViewModel);
            var editedQuestion = await this.DbContext.Questions.FirstOrDefaultAsync(x => x.Id == question.Id);

            Assert.Equal(newText, editedQuestion.Text);
            Assert.False(editedQuestion.IsActive);
        }

        [Fact]
        public async Task DeleteChoiceAsyncShouldDeleteCorrectly()
        {
            var question = await this.CreateQuestionAsync();

            await this.Service.DeleteQuestionAsync(question.Id);

            var questionsCount = this.DbContext.Questions.Where(x => !x.IsDeleted).ToArray().Count();
            var deletedQuestion = await this.DbContext.Questions.FirstOrDefaultAsync(x => x.Id == question.Id);

            Assert.Equal(0, questionsCount);
            Assert.Null(deletedQuestion);
        }

        [Fact]
        public async Task GetQuestionByIdAsyncShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(QuestionViewModel).GetTypeInfo().Assembly);
            
            var examId = Guid.NewGuid().ToString();
            var createQuestionViewModel = new CreateQuestionViewModel
            {
                Text = "question",
                IsActive = true,
                ExamId = examId,
            };

            await this.Service.CreateQuestionAsync(createQuestionViewModel);

            var question = await this.DbContext.Questions.FirstOrDefaultAsync();

            var result = await this.Service.GetQuestionByIdAsync(question.Id);
            var ss = 0;
            Assert.Equal(question.Text, result.Text);
            Assert.Equal(question.IsActive, result.IsActive);
            Assert.Equal(question.ExamId, result.ExamId);
        }

        private async Task<Question> CreateQuestionAsync()
        {
            var question = new Question()
            {
                Text = "question",
                IsActive = true,
                ExamId = Guid.NewGuid().ToString(),
            };

            await this.DbContext.Questions.AddAsync(question);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<Question>(question).State = EntityState.Detached;

            return question;
        }
    }
}
