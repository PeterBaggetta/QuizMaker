using System.Xml.Serialization;

namespace QuizMaker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Quiz Maker!");

            var QuestionList = new List<QuizQuestions>();

            Console.WriteLine("Please enter in a question: ");
            var Question1 = new QuizQuestions();
            Question1.question = Console.ReadLine();

            Console.WriteLine("Enter in your answers separated by commas:");
            string answers = Console.ReadLine();
            Question1.quizAnswers = answers.Split(',').ToList();

            Console.WriteLine("What is the correct answer?");
            Question1.correctAnswer = Convert.ToInt32(Console.ReadLine());

            QuestionList.Add(Question1);

                        
            var Question2 = new QuizQuestions();
            Question2.question = "What colour is grass?";
            Question2.quizAnswers.Add("Blue");
            Question2.quizAnswers.Add("Red");
            Question2.quizAnswers.Add("Yellow");
            Question2.quizAnswers.Add("Green");
            Question2.correctAnswer = 3;

            QuestionList.Add(Question2);




            XmlSerializer serializer = new XmlSerializer(typeof(List<QuizQuestions>));
            var path = @"..\..\..\..\QuizList.xml";
            using (FileStream file = File.Create(path))
            {
                serializer.Serialize(file, QuestionList);
            }
        }
    }
}