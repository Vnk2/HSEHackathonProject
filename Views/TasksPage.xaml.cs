using HSEHackathonProject.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Windows.ApplicationModel.UserDataTasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HSEHackathonProject.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TasksPage : Page
    {
        public async void BuildTaskTree()
        {
            string path = "C:\\Users\\metrochel\\source\\repos\\HSEHackathonProject\\Assets\\Chapters";
            List<Chapter> chapters = await ChaptersTasks.GetTextbookChapters(path);
            foreach (Chapter chapter in chapters)
            {
                TreeViewNode node = new();
                node.Content = chapter;
                foreach (SolvableTask task in chapter.ChapterTasks)
                {
                    TreeViewNode childNode = new();
                    childNode.Content = task;
                    node.Children.Add(childNode);
                }
                TasksView.RootNodes.Add(node);
            }
        }

        public TasksPage()
        {
            InitializeComponent();

            BuildTaskTree();
        }

        private void TasksView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            if (((TreeViewNode)(args.InvokedItem)).Content is SolvableTask task)
            {
                Frame.Navigate(typeof(TaskPage), task);
            }
        }
    }
}
