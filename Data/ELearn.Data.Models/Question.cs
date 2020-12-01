namespace ELearn.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ELearn.Data.Common.Models;

    public class Question : BaseDeletableModel<string>
    {
        public Question()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Choices = new HashSet<Choice>();
        }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string ExamId { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Exam Exam { get; set; }

        public virtual ICollection<Choice> Choices { get; set; }
    }
}
