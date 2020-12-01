namespace ELearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ELearn.Data.Common.Models;

    public class Course : BaseDeletableModel<string>
    {
        public Course()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Exams = new HashSet<Exam>();
            this.Users = new HashSet<ApplicationUser>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
