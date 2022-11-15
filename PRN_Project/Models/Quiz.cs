using System;
using System.Collections.Generic;

namespace PRN_Project.Models
{
    public partial class Quiz
    {
        public Quiz()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuizId { get; set; }
        public int? QuizsetId { get; set; }
        public string? QuizDetail { get; set; }
        public string? QuizType { get; set; }

        public virtual QuizSet? Quizset { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
