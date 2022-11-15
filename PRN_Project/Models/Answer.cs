using System;
using System.Collections.Generic;

namespace PRN_Project.Models
{
    public partial class Answer
    {
        public int AnswerId { get; set; }
        public int QuizId { get; set; }
        public string? AnswerDetail { get; set; }
        public bool Correct { get; set; }

        public virtual Quiz? Quiz { get; set; }
    }
}
