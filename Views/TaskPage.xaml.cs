using HSEHackathonProject.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HSEHackathonProject.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskPage : Page
    {
        public TaskPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is SolvableTask task)
            {
                TaskNoTextBlock.Text = "Задача " + task.TaskNo;
                Uri taskUri = new(task.TaskSource);
                BitmapImage taskImg = new(taskUri);
                Uri answerUri = new(task.AnswerSource);
                BitmapImage answerImg = new(answerUri);

                TaskImage.Source = taskImg;
                AnswerImage.Source = answerImg;
            }
        }

        private void ShowAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAnswerButton.IsEnabled = false;
            AnswerImage.Visibility = Visibility.Visible;
        }

        private async void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            SendBtn.IsEnabled = false;

            Process pythonProcess = new();
            pythonProcess.StartInfo.FileName = "python.exe";
            pythonProcess.StartInfo.RedirectStandardInput = true;
            pythonProcess.StartInfo.RedirectStandardOutput = true;
            pythonProcess.StartInfo.RedirectStandardError = true;
            pythonProcess.StartInfo.UseShellExecute = false;
            pythonProcess.StartInfo.CreateNoWindow = true;
            pythonProcess.StartInfo.Arguments = ApplicationData.Current.LocalFolder.Path + "\\neyro.py";
            pythonProcess.Start();

            string message = MessageTextBox.Text;

            MessagesBox.Text += "Вы: \"" + message + "\"\r\n\r\n";
            pythonProcess.StandardInput.WriteLine(MessageTextBox.Text);
            pythonProcess.StandardInput.Flush();
            pythonProcess.StandardInput.Close();

            pythonProcess.WaitForExit();
            string response = pythonProcess.StandardOutput.ReadLine();

            MessagesBox.Text += response + "\r\n";
            //MessagesBox.Text += await pythonProcess.StandardError.ReadToEndAsync();

            SendBtn.IsEnabled = true;
        }
    }
}
