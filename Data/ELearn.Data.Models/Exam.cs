﻿namespace ELearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ELearn.Common.Enums;
    using ELearn.Data.Common.Models;

    public class Exam : BaseDeletableModel<string>
    {
        public Exam()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Questions = new HashSet<Question>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        [Required]
        public int QuestionsCount { get; set; }

        [Required]
        public OrderType QuestionsOrder { get; set; }

        [Required]
        public OrderType ChoicesOrder { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public virtual ApplicationUser Creator { get; set; }

        [Required]
        public string CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
