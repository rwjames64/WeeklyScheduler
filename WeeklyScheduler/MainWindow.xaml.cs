using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeeklyScheduler.Task;
using WeeklyScheduler.UI;

namespace WeeklyScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeTasks();
        }

        private void InitializeTasks()
        {
            List<string> tasksList = TaskAdapter.GetInstance().TaskTitles();

            foreach (string title in tasksList)
            {
                AddTaskTitleToTasksWrapPanel(title);
            }
        }

        private void AddTaskTitleToTasksWrapPanel(string title)
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem deleteMenuItem = new MenuItem();
            deleteMenuItem.Header = "Delete";
            deleteMenuItem.Tag = title;
            deleteMenuItem.Click += new RoutedEventHandler(TaskItem_DeleteClicked);
            contextMenu.Items.Add(deleteMenuItem);
            Label label = new Label();
            label.Content = title;
            label.MouseLeftButtonDown += new MouseButtonEventHandler(TaskItem_MouseLeftButtonDown);
            label.ContextMenu = contextMenu;

            int index = 0;
            bool found = false;

            while (index < TasksWrapPanel.Children.Count && !found)
            {
                Label child = TasksWrapPanel.Children[index] as Label;

                if (label.Content.ToString().CompareTo(child.Content.ToString()) < 0)
                {
                    found = true;
                }
                else
                {
                    index++;
                }
            }

            TasksWrapPanel.Children.Insert(index, label);
        }

        private void RemoveTaskTitleFromTasksWrapPanel(string title)
        {
            Label label = null;

            foreach (Label child in TasksWrapPanel.Children)
            {
                if (child.Content.ToString() == title)
                {
                    label = child;
                }
            }

            if (label != null)
            {
                TasksWrapPanel.Children.Remove(label);
            }
        }

        private void newScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            nameTextBox.Clear();
            startDatePicker.SelectedDate = null;
        }

        private void newTaskButton_Click(object sender, RoutedEventArgs e)
        {
            NewTaskDialog dialog = new WeeklyScheduler.NewTaskDialog();
            dialog.ShowDialog();

            if (!dialog.cancelled)
            {
                WeeklyScheduler.Task.Task task = new WeeklyScheduler.Task.Task(dialog.titleTextBox.Text, dialog.descriptionTextBox.Text);
                if (TaskAdapter.GetInstance().AddTask(task))
                {
                    AddTaskTitleToTasksWrapPanel(task.Title);
                }
            }
        }

        private void TaskItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;
            DataObject dragData = new DataObject("myFormat", label.Content);
            DragDrop.DoDragDrop(label, dragData, DragDropEffects.Move);
        }

        private void TaskItem_DeleteClicked(object sender, RoutedEventArgs e)
        {
            string title = (sender as MenuItem).Tag as string;
            ConfirmationDialog dialog = new UI.ConfirmationDialog("Confirm Delete", "Are you sure you want to delete task '" + title + "'?");
            dialog.ShowDialog();

            if (!dialog.Cancelled)
            {
                TaskAdapter.GetInstance().RemoveTask(title);
                RemoveTaskTitleFromTasksWrapPanel(title);
            }
        }
    }
}
