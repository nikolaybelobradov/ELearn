namespace ELearn.Services.Data.Tests
{
    using System;

    using AutoMapper;
    using ELearn.Data;
    using ELearn.Data.Common;
    using ELearn.Data.Common.Repositories;
    using ELearn.Data.Models;
    using ELearn.Data.Repositories;
    using ELearn.Services.Data.Choices;
    using ELearn.Services.Data.Courses;
    using ELearn.Services.Data.Dashboard;
    using ELearn.Services.Data.Exams;
    using ELearn.Services.Data.Questions;
    using ELearn.Services.Data.Results;
    using ELearn.Services.Data.Users;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public abstract class BaseServiceTests : IDisposable
    {
        protected BaseServiceTests()
        {
            var services = this.SetServices();

            this.ServiceProvider = services.BuildServiceProvider();
            this.DbContext = this.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        protected IServiceProvider ServiceProvider { get; set; }

        protected ApplicationDbContext DbContext { get; set; }

        public void Dispose()
        {
            this.DbContext.Database.EnsureDeleted();
            this.SetServices();
        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(
                 options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services
                 .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                 {
                     options.Password.RequireDigit = false;
                     options.Password.RequireLowercase = false;
                     options.Password.RequireUppercase = false;
                     options.Password.RequireNonAlphanumeric = false;
                     options.Password.RequiredLength = 6;
                 })
                 .AddEntityFrameworkStores<ApplicationDbContext>();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            services.AddAutoMapper(typeof(ELearnProfile));

            // Application services
            services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
            services.AddTransient(typeof(ILoggerFactory), typeof(LoggerFactory));
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ICoursesService, CoursesService>();
            services.AddTransient<IExamsService, ExamsService>();
            services.AddTransient<IResultsService, ResultsService>();
            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<IChoicesService, ChoicesService>();

            return services;
        }
    }
}
