
namespace QuizMaker
{
    public class Logic
    {
        /// <summary>
        /// Build a Question from pieces the UI gathers
        /// </summary>
        /// <param name="question">Question that the player enters</param>
        /// <param name="answerTexts">Answers that the players enters for the question</param>
        /// <param name="correctAnswer">Correct answer for the question.</param>
        /// <returns>The question that is fully built to be stored in the xml.</returns>
        public static Question BuildQuestion(string question, List<string> answerTexts, List<int> correctAnswer)
        {
            var q = new Question();
            q.question = question;

            for (int i = 0; i < answerTexts.Count; i++)
            {
                var c = new QuizChoices();
                c.answer = answerTexts[i];
                c.isCorrect = correctAnswer.Contains(i);
                q.quizChoices.Add(c);
            }
            return q;
        }

        /// <summary>
        /// Go through the comma separated indexes
        /// Ignore the blanks
        /// Ignore any duplicate answers
        /// </summary>
        /// <param name="input">Player input of the multiple answers</param>
        /// <param name="maxAnswers">Number of answers that the player has entered.</param>
        /// <returns>The list separated out from its commas</returns>
        public static List<int> ParseIndices(string input, int maxAnswers)
        {
            var list = new List<int>();

            if (input == null)
            {
                input = "";
            }
            if (input.Trim().Length == 0)
            {
                return list;
            }

            string[] parts = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int k = 0; k < parts.Length; k++)
            {
                string part = parts[k].Trim();

                int oneBased;
                if (int.TryParse(part, out oneBased))
                {
                    int idx = oneBased - 1;
                    if (idx >= 0 && idx < maxAnswers)
                    {
                        if (!list.Contains(idx))
                        {
                            list.Add(idx); // keep unique values only
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Check if the selected index exactly matches the correct ones
        /// Order does not matter as long as the numbers are the same
        /// </summary>
        /// <param name="q">Question that the player is answering in the game</param>
        /// <param name="selectedIndices">Selected answers that the player is guessing</param>
        /// <returns>If the player answered correctly (True) or not (False)</returns>
        public static bool IsCorrect(Question q, List<int> selectedIndices)
        {
            // Build the list of correct indices from the question
            var correct = new List<int>();
            for (int i = 0; i < q.quizChoices.Count; i++)
            {
                if (q.quizChoices[i].isCorrect)
                {
                    correct.Add(i);
                }
            }

            // Must be the same count first
            if (selectedIndices.Count != correct.Count)
            {
                return false;
            }

            // Ensure every correct index is present in the selected list
            for (int i = 0; i < correct.Count; i++)
            {
                if (!selectedIndices.Contains(correct[i]))
                {
                    return false;
                }
            }

            // No extras possible because counts are equal and we prevented duplicates in ParseIndices
            return true;
        }

        /// <summary>
        /// Shuffle the answers in the question.
        /// </summary>
        /// <param name="q">Question that the player is answering</param>
        /// <param name="rng">Random number to mix up the answers</param>
        public static void ShuffleChoices(Question q, Random rng)
        {
            for (int i = q.quizChoices.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = q.quizChoices[i];
                q.quizChoices[i] = q.quizChoices[j];
                q.quizChoices[j] = tmp;
            }
        }

        /// <summary>
        /// Shuffle the questions in the quiz to be in a different order
        /// </summary>
        /// <param name="questions">List of questions that will be displayed</param>
        /// <param name="rng">Random number toi mix up the answers</param>
        public static void ShuffleQuestions(List<Question> questions, Random rng)
        {
            for (int i = questions.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = questions[i];
                questions[i] = questions[j];
                questions[j] = tmp;
            }
        }
    }
}
