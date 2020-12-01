namespace ELearn.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ELearn.Data.Common.Models;

    public class Choice : BaseDeletableModel<string>
    {
        public Choice()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsTrue { get; set; }

        [Required]
        public string QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }
    }
}
