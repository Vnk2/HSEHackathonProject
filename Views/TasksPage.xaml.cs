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
using Windows.Globalization;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HSEHackathonProject.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TasksPage : Page
    {
        public static TasksPage Instance;

        static bool init = false;
        public async void BuildTaskTree()
        {
            string path = ApplicationData.Current.LocalFolder.Path + "\\AvailableBooks\\irodov";
            TreeViewNode nd = new();
            nd.Content = path;
            TasksView.RootNodes.Add(nd);
            InProgressRing.Visibility = Visibility.Visible;
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
            InProgressRing.Visibility = Visibility.Collapsed;
        }

        private async void CopyTaskTree()
        {
            foreach (TreeViewNode node in Instance.TasksView.RootNodes)
            {
                TasksView.RootNodes.Add(node);
            }
        }

        public TasksPage()
        {
            InitializeComponent();

            if (!init)
            {
                BuildTaskTree();
                init = true;
                Instance = this;
            } else
            {
                CopyTaskTree();
            }
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
