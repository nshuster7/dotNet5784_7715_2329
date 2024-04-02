using BlApi;
using PL.Employee;
using PL.Task;

using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void DataReset(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Press OK to reset the data", "Reseting",
        MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.OK:
                    Factory.Get().ResetDB();
                    break;
                case MessageBoxResult.Cancel:
                    break;
                default:
                    break;
            }
        }

        private void DataInitalization(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Press OK to initalize the data", "Initalizaing",
                MessageBoxButton.OKCancel, MessageBoxImage.Question);
            try
            {
                switch (result)
                {
                    case MessageBoxResult.OK:
                        Factory.Get().InitializeDB();
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewEmployeeListClick(object sender, RoutedEventArgs e)
        {
            new EmployeeListWindow().ShowDialog();
        }
        private void ViewTaskListClick(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().ShowDialog();
        }
        private void ViewGantClick(object sender, RoutedEventArgs e)
        {
            new GantWindow().ShowDialog();
        }
        private void ViewAutomaticScheduleClick(object sender, RoutedEventArgs e)
        {
            new ScheduledWindow().ShowDialog();
        }
    }
}