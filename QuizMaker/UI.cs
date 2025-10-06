
using static QuizMaker.Contants;

namespace QuizMaker
{
    public static class UI
    {
        /// <summary>
        /// Displays the welcome message to the game
        /// </summary>
        public static void DisplayWelcomeMessage()
        {
            Console.WriteLine("=============================================================================");
            Console.WriteLine("                         Welcome to the Quiz Maker!                          ");
            Console.WriteLine("          You can either create your own quiz OR have one imported!          ");
            Console.WriteLine("  After the quiz is either created or imported, you can test your knowledge! ");
            Console.WriteLine("=============================================================================");
        }


        public static bool firstPrint = true;
        /// <summary>
        /// Displays the Menu which tells the player their options for the game
        /// </summary>
        /// <returns>The player choice of mode</returns>
        public static string DisplayMenu()
        {
            if (!firstPrint)
            {
                Console.Clear();
            }
            else
            {
                firstPrint = false;
            }

            Console.WriteLine("===== QuizMaker =====");
            Console.WriteLine($"{EXIT}) Exit Game");
            Console.WriteLine($"{BUILD_QUESTIONS}) Build questions");
            Console.WriteLine($"{PLAY_QUIZ}) Play quiz");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();
            if (choice == null)
            {
                choice = "";
            }
            return choice;
        }

        /// <summary>
        /// This method builds the question when the player chooses to build their questions in the console application.
        /// </summary>
        /// <param name="questionList">Hold the list of questions.</param>
        public static void BuildQuestionsLoop(QuestionStore questionList)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Build Questions ===");
                Console.Write("Enter question (or just Enter to stop): ");
                string question = Console.ReadLine();
                if (question == null)
                {
                    question = "";
                }
                if (question.Trim().Length == 0)
                {
                    break;
                }

                int numOfAnswers = ReadInt($"How many answer quizChoices? ({MIN_ANSWERS}-{MAX_ANSWERS}): ");

                var answerList = new List<string>();
                for (int i = 0; i < numOfAnswers; i++)
                {
                    Console.Write($"Choice {i + 1} answer: ");
                    string answer = Console.ReadLine();
                    if (answer == null)
                    {
                        answer = "";
                    }
                    answerList.Add(answer);
                }

                Console.WriteLine("Enter the number(s) of the correct answer(s), comma-separated (e.g. 2 or 1,3): ");
                string rightAnswers = Console.ReadLine();
                if (rightAnswers == null)
                {
                    rightAnswers = "";
                }
                var correct = Logic.ParseIndices(rightAnswers, numOfAnswers);

                var q = Logic.BuildQuestion(question, answerList, correct);
                questionList.Questions.Add(q);

                Console.Write("Question added. Add another? (Y/N): ");
                ConsoleKeyInfo anotherQuestion = Console.ReadKey();
                char addAnother = char.ToLower(anotherQuestion.KeyChar);
                if (addAnother != ANOTHER)
                {
                    break;
                }
            }

            Console.WriteLine("Saving... Press Enter to return to menu.");
            Console.ReadLine();
        }

        /// <summary>
        /// Play the quiz game. Choose a random question and allow the user to answer each question.
        /// </summary>
        /// <param name="questionList">Hold the list of questions.</param>
        public static void PlayQuizLoop(QuestionStore questionList)
        {
            Console.Clear();
            if (questionList.Questions.Count == 0)
            {
                Console.WriteLine("No questions yet. Go build some first!");
                Console.WriteLine("Press Enter to return.");
                Console.ReadLine();
                return;
            }

            var rand = new Random();
            var questions = new List<Question>(questionList.Questions);
            Logic.ShuffleQuestions(questions, rand);

            int score = 0;

            for (int qIndex = 0; qIndex < questions.Count; qIndex++)
            {
                Console.Clear();
                var q = CloneQuestion(questions[qIndex]);
                Logic.ShuffleChoices(q, rand);

                Console.WriteLine($"Question {qIndex + 1} of {questions.Count}");
                Console.WriteLine(q.question);
                for (int i = 0; i < q.quizChoices.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}) {q.quizChoices[i].answer}");
                }

                Console.WriteLine();
                Console.WriteLine("Select your answer(s) by number (comma-separated if multiple):");
                Console.Write("-> ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    input = "";
                }

                var selected = Logic.ParseIndices(input, q.quizChoices.Count);
                bool correct = Logic.IsCorrect(q, selected);

                if (correct)
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine("Incorrect.");
                    Console.Write("Correct: ");
                    for (int i = 0; i < q.quizChoices.Count; i++)
                    {
                        if (q.quizChoices[i].isCorrect)
                        {
                            Console.Write($"{i + 1} ");
                        }
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("Press Enter for next...");
                Console.ReadLine();
            }

            Console.Clear();
            Console.WriteLine($"Your score: {score} / {questions.Count}");
            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
        }

        /// <summary>
        /// Reads an integer from the console and checks to make sure the number entered is within the bounds of the game.
        /// </summary>
        /// <param name="prompt">The question/prompt which is displayed on the console.</param>
        /// <param name="min">Minimum number in range.</param>
        /// <param name="max">Maximum number in range.</param>
        /// <returns>Returns the number of answers for a question.</returns>
        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine();
                if (s == null)
                {
                    s = "";
                }

                int value;
                if (int.TryParse(s, out value) && value >= MIN_ANSWERS && value <= MAX_ANSWERS)
                {
                    return value;
                }

                Console.WriteLine($"Please enter a number between {MIN_ANSWERS} and {MAX_ANSWERS}.");
            }
        }

        /// <summary>
        /// Clones the question in the list.
        /// </summary>
        /// <param name="original">Original question that was chosen at random.</param>
        /// <returns>Cloned question to display out to the player.</returns>
        private static Question CloneQuestion(Question original)
        {
            var q = new Question();
            q.question = original.question;
            foreach (var c in original.quizChoices)
            {
                var copy = new QuizChoices();
                copy.answer = c.answer;
                copy.isCorrect = c.isCorrect;
                q.quizChoices.Add(copy);
            }
            return q;
        }

        /// <summary>
        /// Displays a Invalid option text to the console for the user.
        /// </summary>
        public static void DisplayInvalid()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("  INVALID OPTION. Press Enter... ");
            Console.WriteLine("=================================");
            Console.ReadLine();
        }
    }
}
