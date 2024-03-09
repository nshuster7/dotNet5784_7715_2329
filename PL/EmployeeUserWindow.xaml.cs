using System.Printing.IndexedProperties;
using System.Windows;
using PL.Task;
namespace PL.Employee
{
    /// <summary>
    /// Interaction logic for EmployeeUserWindow.xaml
    /// </summary>
    public partial class EmployeeUserWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EmployeeUserWindow(int EmployeeUserId)
        {
            InitializeComponent();
            try
            {
                CurrentEmployeeUser = s_bl.Employee.Read(EmployeeUserId)!;
            }
            catch (BO.BlAlreadyExistsException except)
            {
                MessageBoxResult result = MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    new MainWindow().Show();
                }
            }
        }

        public BO.Employee CurrentEmployeeUser
        {
            get { return (BO.Employee)GetValue(CurrentEmployeeUserProperty); }
            set { SetValue(CurrentEmployeeUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentEmployeeUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEmployeeUserProperty =
            DependencyProperty.Register("CurrentEmployeeUser", typeof(BO.Employee), typeof(EmployeeUserWindow), new PropertyMetadata(new BO.Employee()));

        private void UpdateAddClick(object sender, RoutedEventArgs e)
        {//I'm debating whether I need this button. Do with it as you want

        }

        private void EndTask(object sender, RoutedEventArgs e)
        {

        }

        private void ChooseNewTask(object sender, RoutedEventArgs e)
        {
            if (CurrentEmployeeUser is not null) 
            {
                MessageBoxResult result = MessageBox.Show("Do you want to choose a new task?", "message", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) 
                {
                    return;
                }
                EndTask(sender, e);
            }
 //           new TaskListWindow(CurrentEmployeeUser).Show();
        }
    }
}