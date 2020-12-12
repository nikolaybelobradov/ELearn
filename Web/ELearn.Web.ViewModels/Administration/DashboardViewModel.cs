﻿namespace ELearn.Web.ViewModels.Administration
{
    using System.Collections.Generic;

    using ELearn.Data.Models;
    using ELearn.Web.ViewModels.Courses;

    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            this.LastExams = new HashSet<Exam>();
            this.LastResults = new HashSet<Result>();
            this.LastCourses = new HashSet<CourseViewModel>();
        }

        public ICollection<Exam> LastExams { get; set; }

        public ICollection<Result> LastResults { get; set; }

        public ICollection<CourseViewModel> LastCourses { get; set; }
    }
}
