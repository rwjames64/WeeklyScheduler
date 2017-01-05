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
using System.Windows.Shapes;

namespace WeeklyScheduler
{
    /// <summary>
    /// Interaction logic for NewTaskDialog.xaml
    /// </summary>
    public partial class NewTaskDialog : Window
    {
        public bool cancelled { get; private set; }

        public NewTaskDialog()
        {
            InitializeComponent();

            cancelled = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (titleTextBox.Text != "")
            {
                cancelled = false;
                this.Close();
            }
        }
    }
}
