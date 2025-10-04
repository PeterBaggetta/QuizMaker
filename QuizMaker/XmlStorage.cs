using System.Xml.Serialization;

namespace QuizMaker
{
    public class XmlStorage
    {
        public static XmlSerializer serializer = new XmlSerializer(typeof(QuestionStore));

        /// <summary>
        /// Save the user input to a xml file
        /// </summary>
        /// <param name="path">File path of the xml file</param>
        /// <param name="store">The object that stores all of the question and answers</param>
        public static void XmlSave (string path, QuestionStore store)
        {
            using (FileStream file = File.Create(path))
            {
                serializer.Serialize(file, store);
            }
        }

        /// <summary>
        /// Check if the file exists first before opening it - if no file, return empty
        /// Open the xml path
        /// Tries to deserialize the xml into the QuestionStore object
        /// if the xml file is validated and is able to be a QuestionStore object (return the object)
        /// else return empty if the xml file is not properly formatted
        /// </summary>
        /// <param name="path">File path of the xml file</param>
        /// <returns>A QuestionStore object with questions, answers, correct answers or empty object if invalid</returns>
        public static QuestionStore XmlLoad (string path)
        {
            if (!File.Exists(path))
            {
                return new QuestionStore();
            }
 
            using (FileStream file = File.OpenRead(path))
            {
                if (serializer.Deserialize(file) is QuestionStore store)
                {
                    return store;
                }
                else
                {
                    return new QuestionStore();
                }
            }
        }
    }
}
