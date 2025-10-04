
namespace QuizMaker
{
    public class QuestionStore
    {
        public List<Question> Questions = new List<Question>();
    }

    public class Question
    {
        public string question = "";
        public List<QuizChoices> quizChoices = new List<QuizChoices>();
    }

    public class QuizChoices
    {
        public string answer = "";
        public bool isCorrect = false;
    }
}
