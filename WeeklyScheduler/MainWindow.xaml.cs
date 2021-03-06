﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private void AddTaskToDay(ScheduledTask task, StackPanel dayPanel)
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem removeMenuItem = new MenuItem();
            TextBlock textBlock = new TextBlock();
            ScheduledTaskTag tag = new ScheduledTaskTag();

            tag.dayPanel = dayPanel;
            tag.textBlock = textBlock;

            removeMenuItem.Header = "Remove";
            removeMenuItem.Tag = tag;
            removeMenuItem.Click += new RoutedEventHandler(ScheduledTaskItem_RemoveClicked);
            contextMenu.Items.Add(removeMenuItem);

            textBlock.Text = (task.Time != "" ? task.Time + "\n" : "") +
                task.Title + 
                (task.Details == "" ? "" : "\n" + task.Details);

            textBlock.ContextMenu = contextMenu;
            textBlock.Margin = new Thickness(0, 0, 0, 5);
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Tag = task;

            bool indexFound = false;
            int index = 0;

            while (!indexFound && index < dayPanel.Children.Count)
            {
                TextBlock child = dayPanel.Children[index] as TextBlock;
                if ((textBlock.Tag as ScheduledTask).CompareTo(child.Tag as ScheduledTask) < 0)
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
            SundayPanel.Children.Clear();
            MondayPanel.Children.Clear();
            TuesdayPanel.Children.Clear();
            WednesdayPanel.Children.Clear();
            ThursdayPanel.Children.Clear();
            FridayPanel.Children.Clear();
            SaturdayPanel.Children.Clear();
        }

        private void newTaskButton_Click(object sender, RoutedEventArgs e)
        {
            NewTaskDialog dialog = new WeeklyScheduler.NewTaskDialog();
            dialog.ShowDialog();

            if (!dialog.cancelled)
            {
                WeeklyScheduler.Task.Task task = new WeeklyScheduler.Task.Task(dialog.titleTextBox.Text);
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
                int hour = (dialog.useTimeCheckBox.IsChecked == false ? 0 : dialog.hourComboBox.SelectedIndex+1);
                int minute = (dialog.useTimeCheckBox.IsChecked == false ? 0 : dialog.minuteComboBox.SelectedIndex*15);
                string amPm = (dialog.useTimeCheckBox.IsChecked == false ? "" : (dialog.amPmComboBox.SelectedItem as ComboBoxItem).Content.ToString());
                string details = dialog.detailsTextBox.Text;
                ScheduledTask task = new ScheduledTask(title, details, hour, minute, amPm);
                AddTaskToDay(task, dayPanel);
            }
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? nullableDate = startDatePicker.SelectedDate;
            string name = nameTextBox.Text.Trim();
            if (nullableDate != null && name != "")
            {
                DateTime date = nullableDate.Value;
                exportPDF(name, date);
            }
            else
            {
                displayMissingInfoForExportMessageBox(nullableDate, name);
            }
        }

        private static void displayMissingInfoForExportMessageBox(DateTime? nullableDate, string name)
        {
            string message = "The following information is missing:";

            if (name == "")
            {
                message += "\nName";
            }
            if (nullableDate == null)
            {
                message += "\nStart Date";
            }

            MessageBox.Show(message, "Unable to export to PDF");
        }

        private void exportPDF(string name, DateTime date)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "schedule " + date.Year + date.Month.ToString("00") + date.Day.ToString("00");
            dialog.DefaultExt = ".pdf";
            dialog.Filter = "PDF Documents (.pdf)|*.pdf";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    string html = Export.HTMLBuilder.GenerateHTML(name, date, generateListOfScheduledTasks());
                    Export.HTMLConverter.convertHTML(dialog.FileName, html);
                    Debug.WriteLine(html);

                    Process.Start(@dialog.FileName);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "File Error");
                }
            }
        }

        private List<List<ScheduledTask>> generateListOfScheduledTasks()
        {
            List<List<ScheduledTask>> days = new List<List<ScheduledTask>>();
            List<ScheduledTask> sunday = new List<ScheduledTask>();
            List<ScheduledTask> monday = new List<ScheduledTask>();
            List<ScheduledTask> tuesday = new List<ScheduledTask>();
            List<ScheduledTask> wednesday = new List<ScheduledTask>();
            List<ScheduledTask> thursday = new List<ScheduledTask>();
            List<ScheduledTask> friday = new List<ScheduledTask>();
            List<ScheduledTask> saturday = new List<ScheduledTask>();

            fillListWithScheduledTasksForDay(sunday, SundayPanel);
            fillListWithScheduledTasksForDay(monday, MondayPanel);
            fillListWithScheduledTasksForDay(tuesday, TuesdayPanel);
            fillListWithScheduledTasksForDay(wednesday, WednesdayPanel);
            fillListWithScheduledTasksForDay(thursday, ThursdayPanel);
            fillListWithScheduledTasksForDay(friday, FridayPanel);
            fillListWithScheduledTasksForDay(saturday, SaturdayPanel);

            days.Add(sunday);
            days.Add(monday);
            days.Add(tuesday);
            days.Add(wednesday);
            days.Add(thursday);
            days.Add(friday);
            days.Add(saturday);

            return days;
        }

        private void fillListWithScheduledTasksForDay(List<ScheduledTask> tasksForDay, StackPanel dayPanel)
        {
            foreach (TextBlock textBlock in dayPanel.Children)
            {
                tasksForDay.Add(textBlock.Tag as ScheduledTask);
            }
        }
    }
}
