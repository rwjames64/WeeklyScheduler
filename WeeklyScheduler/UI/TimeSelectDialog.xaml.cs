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

namespace WeeklyScheduler.UI
{
    /// <summary>
    /// Interaction logic for TimeSelectDialog.xaml
    /// </summary>
    public partial class TimeSelectDialog : Window
    {
        public bool Cancelled { get; private set; }

        public TimeSelectDialog()
        {
            InitializeComponent();

            Cancelled = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Cancelled = false;
            Close();
        }
    }
}
