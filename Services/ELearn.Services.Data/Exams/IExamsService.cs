﻿namespace ELearn.Services.Data.Exams
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Exams;

    public interface IExamsService
    {
        Task<ExamViewModel> GetExamByIdAsync(string examId);

        Task<ICollection<ExamViewModel>> GetMyExamsAsync(string userId);

        Task CreateExamAsync<TModel>(TModel model);

        Task<ExamViewModel> PrepareExamAsync(string examId);

        Task SaveResultAsync(ExamViewModel viewModel, ApplicationUser currentUser);

        Task<bool> CheckForResultAsync(string examId, string userId);

        Task EditExamAsync(EditExamViewModel model);

        Task DeleteExamAsync(string examId);

        Task CheckForPermissions(ExamViewModel exam, ApplicationUser user);
    }
}
