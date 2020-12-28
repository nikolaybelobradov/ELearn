using ELearn.Services.Data.Exams;
using ELearn.Web.ViewModels.Exams;
using ELearn.Common.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ELearn.Data.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ELearn.Services.Mapping;
using System.Reflection;
using NuGet.Frameworks;
using ELearn.Web.ViewModels.Questions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using ELearn.Web.ViewModels.Choices;
using AutoMapper;

namespace ELearn.Services.Data.Tests
{
    public class ExamsServiceTests : BaseServiceTests
    {
        private IExamsService Service => this.ServiceProvider.GetRequiredService<IExamsService>();

        [Fact]
        public async Task CreateExamAsyncShouldCreateNewExam()
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
            };

            var courseId = Guid.NewGuid().ToString();

            var createExamViewModel = new CreateExamViewModel
            {
                Name = "exam",
                Description = "This is a test exam description.",
                QuestionsCount = 5,
                QuestionsOrder = OrderType.Fixed,
                ChoicesOrder = OrderType.Fixed,
                Creator = user,
                CourseId = courseId,
            };

            await this.Service.CreateExamAsync(createExamViewModel);

            var exam = await this.DbContext.Exams.FirstOrDefaultAsync();
            var examsCount = this.DbContext.Exams.ToArray().Count();

            Assert.Equal(1, examsCount);
            Assert.Equal("exam", exam.Name);
            Assert.Equal("This is a test exam description.", exam.Description);
            Assert.Equal(5, exam.QuestionsCount);
            Assert.Equal(OrderType.Fixed, exam.QuestionsOrder);
            Assert.Equal(OrderType.Fixed, exam.ChoicesOrder);
            Assert.Equal(user, exam.Creator);
            Assert.Equal(courseId, exam.CourseId);
        }

        [Fact]
        public async Task EditExamAsyncShouldEditCorrectly()
        {
            var newName = "New Exam Name";
            var newDescription = "This is a new exam description.";
            var newQuestionsCount = 10;
            var newQuestionsOrder = OrderType.Random;
            var newChoicesOrder = OrderType.Random;

            var exam = await this.CreateExamAsync();

            var editExamViewModel = new EditExamViewModel
            {
                Id = exam.Id,
                Name = newName,
                Description = newDescription,
                QuestionsCount = newQuestionsCount,
                QuestionsOrder = newQuestionsOrder,
                ChoicesOrder = newChoicesOrder,
            };

            await this.Service.EditExamAsync(editExamViewModel);
            var editedExam = await this.DbContext.Exams.FirstOrDefaultAsync(x => x.Id == exam.Id);

            Assert.Equal(newName, editedExam.Name);
            Assert.Equal(newDescription, editedExam.Description);
            Assert.Equal(newQuestionsCount, editedExam.QuestionsCount);
            Assert.Equal(newQuestionsOrder, editedExam.QuestionsOrder);
            Assert.Equal(newChoicesOrder, editedExam.ChoicesOrder);
        }

        [Fact]
        public async Task DeleteExamAsyncShouldDeleteCorrectly()
        {
            var exam = await this.CreateExamAsync();

            await this.Service.DeleteExamAsync(exam.Id);

            var examsCount = this.DbContext.Exams.Where(x => !x.IsDeleted).ToArray().Count();
            var deletedExam = await this.DbContext.Exams.FirstOrDefaultAsync(x => x.Id == exam.Id);

            Assert.Equal(0, examsCount);
            Assert.Null(deletedExam);
        }

        [Fact]
        public async Task CheckIfForResultAsyncWorkCorrectly()
        {
            var userId = Guid.NewGuid().ToString();
            var examId = Guid.NewGuid().ToString();

            var checkForResult = await this.Service.CheckForResultAsync(examId, userId);

            Assert.False(checkForResult);

            await this.AddResultAsync(examId, userId);

            checkForResult = await this.Service.CheckForResultAsync(examId, userId);

            Assert.True(checkForResult);
        }

        [Fact]
        public async Task CheckIfSaveResultAsyncWorkCorrectly()
        {
            var examId = Guid.NewGuid().ToString();

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };

            var choices = new List<ChoiceViewModel>
            {
                new ChoiceViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "choice1",
                    IsTrue = true,
                    IsSelected = true,
                },
                new ChoiceViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "choice2",
                    IsTrue = false,
                    IsSelected = false,
                }
            };

            var questions = new List<QuestionViewModel>
            {
                new QuestionViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "question",
                    IsActive = true,
                    ExamId = examId,
                    Choices = choices,
                }
            };

            var examViewModel = new ExamViewModel
            {
                Id = examId,
                Questions = questions,
            };

            await this.Service.SaveResultAsync(examViewModel, user);

            var result = await this.DbContext.Results.FirstOrDefaultAsync();
            var resultsCount = this.DbContext.Results.ToArray().Count();

            Assert.Equal(1, resultsCount);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(examId, result.ExamId);
            Assert.Equal(100, result.Points);

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => this.Service.SaveResultAsync(examViewModel, user));

            Assert.Equal("You can take an exam only one time!", exception.Message);
            Assert.Equal(1, resultsCount);
        }

        [Fact]
        public async Task CheckIfCheckForPermissionsWorkCorrectly()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };
            var user2 = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
            };

            var exam = new ExamViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Creator = user,
            };

            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() => this.Service.CheckForPermissions(exam, user2));

            Assert.Equal("You do not have permission to perform this action. Please contact an administrator.", exception.Message);

            var checkForException = await Record.ExceptionAsync(() => this.Service.CheckForPermissions(exam, user));

            Assert.Null(checkForException);
        }

        private async Task<Exam> CreateExamAsync()
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
            };
            var exam = new Exam
            {
                Name = "exam",
                Description = "This is a test exam description.",
                QuestionsCount = 5,
                QuestionsOrder = OrderType.Fixed,
                ChoicesOrder = OrderType.Fixed,
                Creator = user,
                CourseId = Guid.NewGuid().ToString(),
            };

            await this.DbContext.Exams.AddAsync(exam);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<Exam>(exam).State = EntityState.Detached;

            return exam;
        }

        private async Task<Exam> AddQuestionsToExamAsync(string examId)
        {
            var exam = await this.DbContext.Exams.FirstOrDefaultAsync(x => x.Id == examId);

            var choicesList1 = new List<Choice>();
            var choicesList2 = new List<Choice>();
            choicesList1.Add(
                new Choice
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "choice1",
                    IsTrue = true,
                });
            choicesList1.Add(
                new Choice
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "choice2",
                    IsTrue = false,
                });
            choicesList2.Add(
                 new Choice
                 {
                     Id = Guid.NewGuid().ToString(),
                     Text = "choice3",
                     IsTrue = true,
                 });
            choicesList2.Add(
                new Choice
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "choice4",
                    IsTrue = false,
                });

            var questions = new List<Question>
            {
                new Question
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "question1",
                    IsActive = true,
                    ExamId = exam.Id,
                    Choices = choicesList1,
                },
                new Question
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "question1",
                    IsActive = true,
                    ExamId = exam.Id,
                    Choices = choicesList2,
                }
            };

            exam.Questions = questions;

            this.DbContext.Update(exam);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<Exam>(exam).State = EntityState.Detached;

            return exam;
        }

        private async Task AddResultAsync(string examId, string userId)
        {
            var result = new Result
            {
                ExamId = examId,
                UserId = userId,
                Points = 100,
            };

            await this.DbContext.Results.AddAsync(result);
            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry<Result>(result).State = EntityState.Detached;
        }
    }
}
