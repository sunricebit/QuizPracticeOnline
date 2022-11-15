using System;
using System.Collections.Generic;

namespace PRN_Project.Models
{
    public partial class history
    {
        public int historyId { get; set; }
        public int? QuizsetId { get; set; }
        public decimal? mark { get; set; }
        public DateTime? dateTime { get; set; }
        public virtual QuizSet? Quizset { get; set; }
    }
}