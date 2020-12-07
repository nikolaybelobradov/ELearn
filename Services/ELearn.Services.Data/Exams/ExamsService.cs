namespace ELearn.Services.Data.Exams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using AutoMapper;
    using ELearn.Common.Enums;
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

        public List<T> RandomElements<T>(ICollection<T> elements)
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

        public async Task<ExamViewModel> PrepareExamAsync(string examId)
        {
            var exam = await this.GetExamByIdAsync(examId);

            exam.Questions = exam.Questions.Where(x => (x.IsActive == true) && (x.Choices.Count > 0)).ToList();

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

        public int CalculateResultAsync(ExamViewModel viewModel)
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
