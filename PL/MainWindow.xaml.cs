using BlApi;
using PL.Employee;
using System.Windows;

namespace PL
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