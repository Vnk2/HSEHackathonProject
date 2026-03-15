using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HSEHackathonProject.Models
{
    
    public class ChaptersTasks
    {
        public ChaptersTasks Instance { get; set; }

        public static async Task<List<Chapter>> GetTextbookChapters(string textbookPath)
        {
            List<Chapter> chaptersList = new();

            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(textbookPath);
            foreach (StorageFolder chapterFolder in await folder.GetFoldersAsync())
            {
                Chapter chapter = await Chapter.Of(chapterFolder.Path);
                chaptersList.Add(chapter);
            }

            return chaptersList;
        }

        public static async Task<List<SolvableTask>> GetChapterTasks(string chapterPath)
        {
            List<SolvableTask> taskList = new List<SolvableTask>();

            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(chapterPath);
            foreach (StorageFolder taskFolder in await folder.GetFoldersAsync())
            {
                SolvableTask? task = await SolvableTask.Of(taskFolder.Path);
                if (task is not null)
                    taskList.Add(task);
            }

            return taskList;
        }

        public ChaptersTasks() {
            if (Instance is null)
            {

                Instance = this;
            }
        }
    }
}
