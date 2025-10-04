
namespace QuizMaker
{
    public class Logic
    {
        // Build a Question from pieces the UI gathers
        public static Question BuildQuestion(string prompt, List<string> choiceTexts, List<int> correctIndices)
        {
            var q = new Question();
            q.question = prompt;

            for (int i = 0; i < choiceTexts.Count; i++)
            {
                var c = new QuizChoices();
                c.answer = choiceTexts[i];
                c.isCorrect = ListContains(correctIndices, i); // 0-based index
                q.quizChoices.Add(c);
            }
            return q;
        }

        // Parse comma-separated indices like "1,3" to a LIST of 0-based indices.
        // - ignores blanks
        // - ignores out-of-range numbers
        // - avoids duplicates
        public static List<int> ParseIndices(string input, int maxChoices)
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
                    int idx = oneBased - 1; // convert 1..N from the UI into 0..N-1
                    if (idx >= 0 && idx < maxChoices)
                    {
                        if (!ListContains(list, idx))
                        {
                            list.Add(idx); // keep unique values only
                        }
                    }
                }
            }
            return list;
        }

        // Check if the selected indices EXACTLY match the correct ones (order doesn’t matter)
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
                if (!ListContains(selectedIndices, correct[i]))
                {
                    return false;
                }
            }

            // No extras possible because counts are equal and we prevented duplicates in ParseIndices
            return true;
        }

        // Shuffle choices in-place (Fisher–Yates)
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

        // Shuffle questions in-place
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

        // ---- tiny helper (avoids LINQ) ----
        private static bool ListContains(List<int> list, int value)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i] == value)
                {
                    return true;
                }
            return false;
        }
    }
}
