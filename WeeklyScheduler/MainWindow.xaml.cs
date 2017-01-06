﻿using System;
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
            Label label = new Label();
            label.Content = title;
            label.MouseLeftButtonDown += new MouseButtonEventHandler(TaskItem_MouseLeftButtonDown);

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
    }
}
