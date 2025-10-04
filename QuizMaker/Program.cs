
using System;

namespace QuizMaker
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string path = @"..\..\..\..\QuizList.xml";

            var store = XmlStorage.XmlLoad(path);

            while (true)
            {
                string userChoice = UI.DisplayMenu();

                switch (userChoice)
                {
                    case "1":
                        UI.BuildQuestionsLoop(store);
                        XmlStorage.XmlSave(path, store);
                        break;

                    case "2":
                        UI.PlayQuizLoop(store);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option. Press Enter...");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}