
using static QuizMaker.Contants;

namespace QuizMaker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UI.DisplayWelcomeMessage();

            var questionList = XmlStorage.XmlLoad(PATH);

            bool firstPrint = true;

            while (true)
            {
                UI.DisplayMenu(firstPrint);
                string userChoice = UI.GetUserChoice();

                switch (userChoice)
                {
                    case BUILD_QUESTIONS:
                        UI.BuildQuestionsLoop(questionList);
                        XmlStorage.XmlSave(PATH, questionList);
                        firstPrint = false;
                        break;

                    case PLAY_QUIZ:
                        UI.PlayQuizLoop(questionList);
                        firstPrint = false;
                        break;

                    case EXIT:
                        return;

                    default:
                        UI.DisplayInvalid();
                        firstPrint = false;
                        break;
                }
            }
        }
    }
}