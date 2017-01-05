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
                // TODO
                Label label = new Label();
                label.Content = dialog.titleTextBox.Text;
                TasksWrapPanel.Children.Add(label);
            }
        }
    }
}
