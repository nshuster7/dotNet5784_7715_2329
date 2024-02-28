using BlApi;
using PL.Employee;
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

        private void ViewEmployeeListClick(object sender, RoutedEventArgs e)
        {
            new EmployeeListWindow().ShowDialog();
        }

        private void DataInitalization(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Press OK to initalize the data", "Initalizaing",
                MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.OK:
                    Factory.Get().ResetDB();
                    Factory.Get().InitializeDB();
                    break; 
                case MessageBoxResult.Cancel:
                    break;
                default: 
                    break; 
            }
        }
    }
}