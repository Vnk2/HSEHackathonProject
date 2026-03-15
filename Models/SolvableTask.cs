using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Media;

namespace HSEHackathonProject.Models
{
    public struct XMLTaskMetadataTag
    {
        public string TaskNo;
    }
    public struct XMLTaskTag
    {
        public string Source;
    }

    public struct XMLAnswerTag
    {
        public string Source;
    }

    public struct XMLCorrectAnswerTag
    {
        public string Type;
        public string Content;
    }
    public class SolvableTask
    {
        public string TaskNo { get; set; }
        public string TaskSource { get; set; }
        public string AnswerSource { get; set; }

        public string RightAnswer { get; set; }

        public async static Task<SolvableTask?> Of(string resourceFolder)
        {
                XMLTaskMetadataTag meta = new();
                XMLAnswerTag answer = new();
                XMLTaskTag task = new();
                XMLCorrectAnswerTag correctAnswer = new();

                XmlReaderSettings settings = new();
                settings.Async = true;
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;

                XmlReader xml = XmlReader.Create(resourceFolder + "\\meta.xml", settings);
                await xml.MoveToContentAsync();
                while (xml.MoveToNextAttribute())
                {
                    if (xml.Name == "TaskNo")
                    {
                        meta.TaskNo = xml.Value;
                    }
                }

                await xml.MoveToContentAsync();
                while (await xml.ReadAsync())
                {
                    if (xml.NodeType != XmlNodeType.Element)
                        continue;

                    switch (xml.Name)
                    {
                        case "Task":
                            while (xml.MoveToNextAttribute())
                            {
                                if (xml.Name == "Source")
                                    task.Source = xml.Value;
                            }
                            xml.MoveToElement();
                            break;
                        case "Answer":
                            while (xml.MoveToNextAttribute())
                            {
                                if (xml.Name == "Source")
                                    answer.Source = xml.Value;
                            }
                            xml.MoveToElement();
                            break;
                        case "CorrectAnswer":
                            while (xml.MoveToNextAttribute())
                            {
                                if (xml.Name == "Type")
                                    correctAnswer.Type = xml.Value;
                            }
                            xml.Read();
                            correctAnswer.Content = xml.Value.Trim();
                            xml.MoveToElement();
                            break;
                    }
                }

                SolvableTask stask = new();
                stask.TaskSource = resourceFolder + "\\" + task.Source;
                stask.AnswerSource = resourceFolder + "\\" + answer.Source;
                stask.RightAnswer = correctAnswer.Content;
                stask.TaskNo = meta.TaskNo;

                return stask;
        }

        public override string ToString()
        {
            return TaskNo;
        }
    }
}
