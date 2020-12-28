namespace ELearn.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Services.Data.Choices;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Choices;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class ChoicesServiceTests : BaseServiceTests
    {
        private IChoicesService Service => this.ServiceProvider.GetRequiredService<IChoicesService>();

        [Fact]
        public async Task AddChoiceAsyncShouldAddNewChoiceInDb()
        {
            var questionId = Guid.NewGuid().ToString();
            var addChoiceViewModel = new AddChoiceViewModel
            {
                Text = "choice",
                IsTrue = true,
                QuestionId = questionId,
            };

            await this.Service.AddChoiceAsync(addChoiceViewModel);

            var choice = await this.DbContext.Choices.FirstOrDefaultAsync();
            var choicesCount = this.DbContext.Choices.ToArray().Count();

            Assert.Equal(1, choicesCount);
            Assert.Equal("choice", choice.Text);
            Assert.True(choice.IsTrue);
            Assert.Equal(questionId, choice.QuestionId);
        }

        [Fact]
        public async Task EditChoiceAsyncShouldEditCorrectly()
        {
            var newText = "New Choice Text";
            var newIsTrue = false;

            var choice = await this.AddChoiceAsync();

            var editChoiceViewModel = new EditChoiceViewModel
            {
                Id = choice.Id,
                Text = newText,
                IsTrue = newIsTrue,
            };

            await this.Service.EditChoiceAsync(editChoiceViewModel);
            var editedChoice = await this.DbContext.Choices.FirstOrDefaultAsync(x => x.Id == choice.Id);

            Assert.Equal(newText, editedChoice.Text);
            Assert.False(editedChoice.IsTrue);
        }

        [Fact]
        public async Task DeleteChoiceAsyncShouldDeleteCorrectly()
        {
            var choice = await this.AddChoiceAsync();

            await this.Service.DeleteChoiceAsync(choice.Id);

            var choicesCount = this.DbContext.Choices.Where(x => !x.IsDeleted).ToArray().Count();
            var deletedChoice = await this.DbContext.Choices.FirstOrDefaultAsync(x => x.Id == choice.Id);

            Assert.Equal(0, choicesCount);
            Assert.Null(deletedChoice);
        }

        [Fact]
        public async Task GetChoiceByIdAsyncShouldWorkCorrectly()
        {
            var choice = await this.AddChoiceAsync();

            AutoMapperConfig.RegisterMappings(typeof(ChoiceViewModel).GetTypeInfo().Assembly);
            var result = await this.Service.GetChoiceByIdAsync(choice.Id);

            Assert.Equal(choice.Id, result.Id);
            Assert.Equal(choice.Text, result.Text);
            Assert.Equal(choice.IsTrue, result.IsTrue);
            Assert.Equal(choice.QuestionId, result.QuestionId);
        }

        private async Task<Choice> AddChoiceAsync()
        {
            var choice = new Choice()
            {
                Text = "choice",
                IsTrue = true,
                QuestionId = Guid.NewGuid().ToString(),
            };

            await this.DbContext.Choices.AddAsync(choice);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<Choice>(choice).State = EntityState.Detached;

            return choice;
        }
    }
}
