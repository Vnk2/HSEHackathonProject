using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HSEHackathonProject.Models
{
    public struct XMLChapterMetaTag
    {
        public string Name { get; set; }
    }
    public class Chapter
    {
        public string Name { get; set; }
        public List<SolvableTask> ChapterTasks;

        public static async Task<Chapter> Of(string chapterPath)
        {
            XMLChapterMetaTag tag = new();
            XmlReaderSettings settings = new();
            settings.Async = true;
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            XmlReader reader = XmlReader.Create(chapterPath + "\\chaptermeta.xml", settings);
            await reader.MoveToContentAsync();
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "Name")
                    tag.Name = reader.Value;
            }
            reader.Close();

            Chapter chapter = new Chapter();
            chapter.ChapterTasks = await ChaptersTasks.GetChapterTasks(chapterPath);
            chapter.Name = tag.Name;
            return chapter;
        }

        public override string ToString()
        {
            string ret = "Chapter\r\n";
            ret += "Name = " + Name + "\r\n";
            foreach (var task in ChapterTasks)
            {
                ret += task.ToString();
            }
            return ret;
        }
    }
}
