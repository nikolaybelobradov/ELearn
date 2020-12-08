namespace ELearn.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ELearn.Data.Common.Models;

    public class Result : BaseDeletableModel<string>
    {
        public Result()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public int Points { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string ExamId { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Exam Exam { get; set; }
    }
}
