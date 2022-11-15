using System;
using System.Collections.Generic;

namespace PRN_Project.Models
{
    public partial class QuizSet
    {
        public QuizSet()
        {
            Quizzes = new HashSet<Quiz>();
        }

        public int QuizsetId { get; set; }
        public string? QuizsetTitle { get; set; }
        public string? Category { get; set; }
        public decimal? Score { get; set; }
        public int? Time { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<history> historis { get; set; }
    }
}
