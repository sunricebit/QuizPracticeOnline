namespace PRN_Project.Models
{
    public class QuizUserChoice
    {
        public int quizId { get; set; }
        public List<string> choice { get; set; }

        public QuizUserChoice(int quizId, List<string> choice)
        {
            this.quizId = quizId;
            this.choice = choice;
        }

        public QuizUserChoice()
        {
        }
    }
}
