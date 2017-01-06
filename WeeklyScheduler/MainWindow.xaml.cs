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
        private class ScheduledTaskTag
        {
            public TextBlock textBlock;
            public StackPanel dayPanel;
        }

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
            label.Background = Brushes.LightGray;
            label.Content = title;
            label.MouseLeftButtonDown += new MouseButtonEventHandler(TaskItem_MouseLeftButtonDown);
            label.Margin = new Thickness(5, 5, 0, 0);
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

        private void AddTaskToDay(string taskTitle, StackPanel dayPanel, int hour, int minute, string amPm)
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem removeMenuItem = new MenuItem();
            WeeklyScheduler.Task.Task task = TaskAdapter.GetInstance().GetTask(taskTitle);
            TextBlock textBlock = new TextBlock();
            ScheduledTaskTag tag = new ScheduledTaskTag();

            tag.dayPanel = dayPanel;
            tag.textBlock = textBlock;

            removeMenuItem.Header = "Remove";
            removeMenuItem.Tag = tag;
            removeMenuItem.Click += new RoutedEventHandler(ScheduledTaskItem_RemoveClicked);
            contextMenu.Items.Add(removeMenuItem);

            string hourAndMinute = hour.ToString("00") + ":" + minute.ToString("00");

            textBlock.Text = hourAndMinute + " " + amPm + "\n" +
                task.Title + "\n" +
                task.Description;

            textBlock.ContextMenu = contextMenu;
            textBlock.Margin = new Thickness(0, 0, 0, 5);
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Tag = amPm + hourAndMinute;

            bool indexFound = false;
            int index = 0;

            while (!indexFound && index < dayPanel.Children.Count)
            {
                TextBlock child = dayPanel.Children[index] as TextBlock;
                if ((textBlock.Tag as string).CompareTo(child.Tag as string) < 0)
                {
                    indexFound = true;
                }
                else
                {
                    index++;
                }
            }

            dayPanel.Children.Insert(index, textBlock);
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
            DataObject dragData = new DataObject("title", label.Content.ToString());
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

        private void ScheduledTaskItem_RemoveClicked(object sender, RoutedEventArgs e)
        {
            string title = (sender as MenuItem).Tag as string;
            ConfirmationDialog dialog = new UI.ConfirmationDialog("Confirm Remove", "Are you sure you want to remove task from schedule?");
            dialog.ShowDialog();

            if (!dialog.Cancelled)
            {
                ScheduledTaskTag tag = (sender as MenuItem).Tag as ScheduledTaskTag;
                tag.dayPanel.Children.Remove(tag.textBlock);
            }
        }

        private void TaskItem_Drop(object sender, DragEventArgs e)
        {
            TimeSelectDialog dialog = new TimeSelectDialog();
            dialog.ShowDialog();

            if (!dialog.Cancelled)
            {
                string title = e.Data.GetData("title") as string;
                StackPanel dayPanel = sender as StackPanel;
                int hour = (dialog.noTimeCheckBox.IsChecked == true ? 0 : dialog.hourComboBox.SelectedIndex+1);
                int minute = (dialog.noTimeCheckBox.IsChecked == true ? 0 : dialog.minuteComboBox.SelectedIndex*15);
                string amPm = (dialog.noTimeCheckBox.IsChecked == true ? "" : (dialog.amPmComboBox.SelectedItem as ComboBoxItem).Content.ToString());
                AddTaskToDay(title, dayPanel, hour, minute, amPm);
            }
        }
    }
}
