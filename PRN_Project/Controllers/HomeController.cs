using Microsoft.AspNetCore.Mvc;
using PRN_Project.Models;
using System.Collections.Generic;

namespace PRN_Project.Controllers
{
    public class HomeController : Controller
    {
        static QuizSet q = new QuizSet();
        static List<Quiz> lstQuiz = new List<Quiz>();
        public prn_projectContext context = new prn_projectContext();
        static List<QuizUserChoice> quizUserChoices = new List<QuizUserChoice>();
        static List<history> lstHistory = new List<history>();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult QuizSetView()
        {
            List<QuizSet> quizSets = context.QuizSets.ToList();
            ViewBag.QuizSets = quizSets;
            return View();

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BeforeDoTest(string quizsetId)
        {
            lstQuiz.Clear();
            q = context.QuizSets.FirstOrDefault(item => item.QuizsetId == Int32.Parse(quizsetId));
            quizUserChoices = new List<QuizUserChoice>();
            context.Quizzes.ToList();
            List<Quiz> lstQuestion = new List<Quiz>();
            context.Answers.ToList();
            int numberQuestion = q.Quizzes.Count();
            for (int i = 0; i < numberQuestion; i++)
            {
                Quiz a = q.Quizzes.First();
                q.Quizzes.Remove(a);
                lstQuestion.Add(a);
            }

            for (int i = 0; i < numberQuestion; i++)
            {
                Random rand = new Random();
                int temp = 0;
                temp = rand.Next(lstQuestion.Count);
                string s = lstQuestion[temp].QuizDetail;
                q.Quizzes.Add(lstQuestion[temp]);
                lstQuiz.Add(lstQuestion[temp]);
                lstQuestion.Remove(lstQuestion[temp]);
            }
            ViewBag.Time = q.Time;
            ViewBag.QuizSet = q;
            ViewBag.IndexInList = 1;
            return View();
        }

        [HttpPost]
        public IActionResult Create(QuizSet quiz, List<Quiz> lstQuiz, List<Answer> lstAnswer)
        {

            if (ModelState.IsValid)
            {
                context.QuizSets.Add(quiz);
                foreach (Quiz q in lstQuiz)
                {
                    context.Quizzes.Add(q);
                }
                foreach (Answer a in lstAnswer)
                {
                    context.Answers.Add(a);
                }
            }
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Practice(string next, List<string> choice, string submit, string quizId, string toQuestion, string remainTime)
        {
            if (quizId != null)
            {
                QuizUserChoice fin = quizUserChoices.FirstOrDefault(item => item.quizId == Int32.Parse(quizId));
                if (fin != null)
                {
                    quizUserChoices.Remove(fin);
                }
                QuizUserChoice quizUserChoice = new QuizUserChoice(Int32.Parse(quizId), choice);
                quizUserChoices.Add(quizUserChoice);
            }
            if (toQuestion == null)
            {
                if ((Int32.Parse(next) - 1) == lstQuiz.Count || submit == "Submit Answer")
                {
                    ViewBag.AnswerSubmited = quizUserChoices;
                    ViewBag.QuizSets = lstQuiz;
                    return View("BeforeViewResult");
                }
                else
                {
                    Quiz quiz = lstQuiz[Int32.Parse(next) - 1];
                    context.Answers.ToList();
                    ViewBag.Question = quiz;
                    ViewBag.Time = remainTime;
                    ViewBag.Next = Int32.Parse(next) + 1;
                    ViewBag.IndexInList = Int32.Parse(next);
                    ViewBag.NumOfQuestion = lstQuiz.Count;
                    return View();
                }
            }
            else
            {
                Quiz quiz = lstQuiz[Int32.Parse(toQuestion) - 1];
                context.Answers.ToList();
                QuizUserChoice lastChoice = quizUserChoices.FirstOrDefault(item => item.quizId == quiz.QuizId);
                ViewBag.Choosen = lastChoice;
                ViewBag.Question = quiz;
                ViewBag.Time = remainTime;
                ViewBag.Next = Int32.Parse(toQuestion) + 1;
                ViewBag.IndexInList = Int32.Parse(toQuestion);
                ViewBag.NumOfQuestion = lstQuiz.Count;
                return View();
            }
        }

        public IActionResult ViewQuizSet(string id)
        {

            var lstQuiz = context.Quizzes.Where(item => item.QuizsetId == Int32.Parse(id)).ToList();
            foreach (Quiz q in lstQuiz)
            {
                string a = q.QuizDetail;
            }
            context.Answers.ToList();
            ViewBag.QuizSets = lstQuiz;
            for(int i = 0; i < lstQuiz.Count; i++)
            {
                lstQuiz.Remove(lstQuiz.First());
            }
            return View();
        }

        [HttpGet]
        public IActionResult ViewResult()
        {
            float numberOfCorrectAnswer = 0;
            foreach (Quiz quiz in lstQuiz)
            {
                int x = quiz.QuizId;
                foreach (QuizUserChoice quizUserChoice in quizUserChoices)
                {
                    int y = quizUserChoice.quizId;
                    if (quizUserChoice.quizId == quiz.QuizId)
                    {
                        if (quizUserChoice.choice.Count == 0) { continue; }
                        if (quiz.QuizType.Equals("Radio"))
                        {
                            Answer a = quiz.Answers.FirstOrDefault(item => item.Correct == true);
                            if (a.AnswerDetail == quizUserChoice.choice.First())
                            {
                                string s = quizUserChoice.choice.First();
                                numberOfCorrectAnswer++;
                            }
                            else
                            {
                                string s = quizUserChoice.choice.First();
                                string s1 = a.AnswerDetail;
                            }
                        }
                        if (quiz.QuizType.Equals("Check"))
                        {

                            List<Answer> a = quiz.Answers.Where(item => item.Correct == true).ToList();
                            if (a.Count == quizUserChoice.choice.Count)
                            {
                                bool correct = true;
                                for (int i = 0; i < quizUserChoice.choice.Count; i++)
                                {
                                    if (a[i].AnswerDetail != quizUserChoice.choice[i])
                                    {
                                        correct = false;
                                        break;
                                    }
                                }
                                if (correct) numberOfCorrectAnswer++;
                            }

                        }
                    }

                }
            }
            context.Answers.ToList();
            decimal result = (decimal)(numberOfCorrectAnswer / lstQuiz.Count) * 10;
            q.Score = result;
            prn_projectContext context1 = new prn_projectContext();
            QuizSet quizToUpdate = context1.QuizSets.FirstOrDefault(item => item.QuizsetId == q.QuizsetId);
            quizToUpdate.Score = result;
            history nhistory = new history();
            nhistory.dateTime = DateTime.Now;
            nhistory.mark = result;
            nhistory.QuizsetId = q.QuizsetId;
            nhistory.historyId = lstHistory.Count + 1;
            lstHistory.Add(nhistory);
            context1.QuizSets.Update(quizToUpdate);
            context1.SaveChanges();
            ViewBag.Result = result;
            ViewBag.QuizSets = lstQuiz;
            ViewBag.QuizUserChoices = quizUserChoices;
            return View();
        }

        [HttpGet]
        public IActionResult ChooseTime()
        {
            ViewBag.IndexInList = 1;
            return View();
        }
        public IActionResult ViewHistory()
        {
            ViewBag.History = lstHistory;
            return View();
        }
    }
}
