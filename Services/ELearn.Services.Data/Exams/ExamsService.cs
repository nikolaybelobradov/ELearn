namespace ELearn.Services.Data.Exams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using ELearn.Common;
    using ELearn.Common.Enums;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Services.Mapping;
    using ELearn.Web.ViewModels.Exams;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ExamsService : IExamsService
    {
        private readonly IDeletableEntityRepository<Exam> examRepository;
        private readonly IDeletableEntityRepository<Result> resultRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ExamsService(
                IDeletableEntityRepository<Exam> examRepository,
                IDeletableEntityRepository<Result> resultRepository,
                UserManager<ApplicationUser> userManager,
                IMapper mapper)
        {
            this.examRepository = examRepository;
            this.resultRepository = resultRepository;
            this.userManager = userManager;
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

        public async Task<ExamViewModel> PrepareExamAsync(string examId)
        {
            var exam = await this.GetExamByIdAsync(examId);

            exam.Questions = exam.Questions
                .Where(x => (x.IsActive == true) && (x.Choices.Count > 0) && (x.Choices.Where(y => y.IsTrue == true).Count() > 0))
                .ToList();

            if (exam.QuestionsOrder == OrderType.Random)
            {
                exam.Questions = this.RandomElements(exam.Questions);
            }
            else
            {
                exam.Questions.OrderBy(x => x.CreatedOn);
            }

            if (exam.ChoicesOrder == OrderType.Random)
            {
                foreach (var question in exam.Questions)
                {
                    question.Choices = this.RandomElements(question.Choices);
                }
            }
            else
            {
                foreach (var question in exam.Questions)
                {
                    question.Choices.OrderBy(x => x.CreatedOn);
                }
            }

            if (exam.Questions.Count > exam.QuestionsCount)
            {
                exam.Questions = exam.Questions.Take(exam.QuestionsCount).ToList();
            }

            return exam;
        }

        public async Task SaveResultAsync(ExamViewModel viewModel, ApplicationUser currentUser)
        {
            var checkForResult = await this.CheckForResultAsync(viewModel.Id, currentUser.Id);

            if (!checkForResult)
            {
                int points = this.CalculateResult(viewModel);

                var result = new Result()
                {
                    Points = points,
                    ExamId = viewModel.Id,
                    UserId = currentUser.Id,
                };

                await this.resultRepository.AddAsync(result);
                await this.resultRepository.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("You can take an exam only one time!");
            }
        }

        public async Task<bool> CheckForResultAsync(string examId, string userId)
        {
            var result = await this.resultRepository.All()
                .FirstOrDefaultAsync(x => (x.ExamId == examId) && (x.UserId == userId));

            if (result != null)
            {
                return true;
            }

            return false;
        }

        public async Task EditExamAsync(EditExamViewModel model)
        {
            var exam = await this.examRepository.All()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            exam.Name = model.Name;
            exam.Description = model.Description;
            exam.QuestionsCount = model.QuestionsCount;
            exam.QuestionsOrder = model.QuestionsOrder;
            exam.ChoicesOrder = model.ChoicesOrder;

            this.examRepository.Update(exam);
            await this.examRepository.SaveChangesAsync();
        }

        public async Task DeleteExamAsync(string examId)
        {
            var exam = await this.examRepository.All()
                .FirstOrDefaultAsync(x => x.Id == examId);

            this.examRepository.Delete(exam);
            await this.examRepository.SaveChangesAsync();
        }

        public async Task CheckForPermissions(ExamViewModel exam, ApplicationUser user)
        {
            if (!await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName) && exam.Creator.Id != user.Id)
            {
                throw new ArgumentException("You do not have permission to perform this action. Please contact an administrator.");
            }
        }

        public async Task<ICollection<ExamViewModel>> GetMyExamsAsync(string userId)
        {
            var exams = await this.examRepository
                .All()
                .Where(x => x.CreatorId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<ExamViewModel>()
                .ToListAsync();

            return exams;
        }

        private List<T> RandomElements<T>(ICollection<T> elements)
        {
            var list = elements.ToList();
            Random random = new Random();

            for (int i = 0; i < elements.Count; i++)
            {
                int swapIndex = random.Next(i, elements.Count);
                if (swapIndex != i)
                {
                    var temp = list[i];
                    list[i] = list[swapIndex];
                    list[swapIndex] = temp;
                }
            }

            return list;
        }

        private int CalculateResult(ExamViewModel viewModel)
        {
            var trueQuestionsCount = 0;

            foreach (var question in viewModel.Questions)
            {
                var trueChoicesCount = question.Choices.Where(x => x.IsTrue == true).Count();
                var counter = 0;

                foreach (var choice in question.Choices)
                {
                    if (choice.IsSelected && choice.IsTrue)
                    {
                        counter++;
                    }
                }

                if (trueChoicesCount == counter)
                {
                    trueQuestionsCount++;
                }
            }

            var result = (trueQuestionsCount * 100) / viewModel.Questions.Count;

            return result;
        }
    }
}
