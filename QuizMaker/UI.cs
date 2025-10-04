
using System.Security.Cryptography.X509Certificates;

namespace QuizMaker
{
    public static class UI
    {
        public static void DisplayWelcomeMessage()
        {
            Console.WriteLine("=============================================================================");
            Console.WriteLine("                         Welcome to the Quiz Maker!                          ");
            Console.WriteLine("          You can either create your own quiz OR have one imported!          ");
            Console.WriteLine("  After the quiz is either created or imported, you can test your knowledge! ");
            Console.WriteLine("=============================================================================");
        }

        public static bool firstPrint = true;
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

            Console.Clear();
            Console.WriteLine("===== QuizMaker =====");
            Console.WriteLine("1) Build questions");
            Console.WriteLine("2) Play quiz");
            Console.WriteLine("0) Exit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();
            if (choice == null)
            {
                choice = "";
            }
            return choice;
        }
        public static void BuildQuestionsLoop(QuestionStore store)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Build Questions ===");
                Console.Write("Enter question prompt (or just Enter to stop): ");
                string prompt = Console.ReadLine();
                if (prompt == null)
                {
                    prompt = "";
                }
                if (prompt.Trim().Length == 0)
                {
                    break;
                }

                int choiceCount = ReadInt("How many answer quizChoices? (2-10): ", 2, 10);

                var answers = new List<string>();
                for (int i = 0; i < choiceCount; i++)
                {
                    Console.Write("Choice " + (i + 1) + " answer: ");
                    string answer = Console.ReadLine();
                    if (answer == null)
                    {
                        answer = "";
                    }
                    answers.Add(answer);
                }

                Console.WriteLine("Enter the number(s) of the correct answer(s), comma-separated (e.g., 2 or 1,3): ");
                Console.Write("> ");
                string raw = Console.ReadLine();
                if (raw == null)
                {
                    raw = "";
                }
                var correct = Logic.ParseIndices(raw, choiceCount);

                var q = Logic.BuildQuestion(prompt, answers, correct);
                store.Questions.Add(q);

                Console.Write("Question added. Add another? (y/n): ");
                string again = Console.ReadLine();
                if (again == null)
                {
                    again = "";
                }
                if (!again.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }

            Console.WriteLine("Saving... Press Enter to return to menu.");
            Console.ReadLine();
        }

        public static void PlayQuizLoop(QuestionStore store)
        {
            Console.Clear();
            if (store.Questions.Count == 0)
            {
                Console.WriteLine("No questions yet. Go build some first!");
                Console.WriteLine("Press Enter to return.");
                Console.ReadLine();
                return;
            }

            var rng = new Random();
            var questions = new List<Question>(store.Questions);
            Logic.ShuffleQuestions(questions, rng);

            int score = 0;

            for (int qi = 0; qi < questions.Count; qi++)
            {
                Console.Clear();
                var q = CloneQuestion(questions[qi]);
                Logic.ShuffleChoices(q, rng);

                Console.WriteLine("Question " + (qi + 1) + " of " + questions.Count);
                Console.WriteLine(q.question);
                for (int i = 0; i < q.quizChoices.Count; i++)
                {
                    Console.WriteLine("  " + (i + 1) + ") " + q.quizChoices[i].answer);
                }

                Console.WriteLine();
                Console.WriteLine("Select your answer(s) by number (comma-separated if multiple):");
                Console.Write("> ");
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
                            Console.Write((i + 1) + " ");
                        }
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("Press Enter for next...");
                Console.ReadLine();
            }

            Console.Clear();
            Console.WriteLine("Your score: " + score + "/" + questions.Count);
            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
        }

        private static int ReadInt(string prompt, int min, int max)
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
                if (int.TryParse(s, out value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine("Please enter a number between " + min + " and " + max + ".");
            }
        }

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
    }
}
