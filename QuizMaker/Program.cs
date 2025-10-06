
using static QuizMaker.Contants;

namespace QuizMaker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UI.DisplayWelcomeMessage();

            var questionList = XmlStorage.XmlLoad(PATH);

            while (true)
            {
                string userChoice = UI.DisplayMenu();

                switch (userChoice)
                {
                    case BUILD_QUESTIONS:
                        UI.BuildQuestionsLoop(questionList);
                        XmlStorage.XmlSave(PATH, questionList);
                        break;

                    case PLAY_QUIZ:
                        UI.PlayQuizLoop(questionList);
                        break;

                    case EXIT:
                        return;

                    default:
                        UI.DisplayInvalid();
                        break;
                }
            }
        }
    }
}