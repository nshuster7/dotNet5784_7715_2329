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
        int ID;
        public EmployeeUserWindow(int EmployeeUserId)
        {
            InitializeComponent();
            try
            {
                 UserID = EmployeeUserId;
                 ID = BO.Tools.GetCurrentTaskId(UserID) ??0;
                if(ID == 0)
                    MessageBox.Show($"Employee with ID {UserID} does not work on a task", "message", MessageBoxButton.OK);
                else
                    CurrentTask = s_bl.Task.Read(ID)!;
            }
            catch (BO.BlDoesNotExistException except)
            {
                MessageBoxResult result = MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    new MainWindow().Show();
                }
            }
        }
        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(EmployeeUserWindow), new PropertyMetadata(null));


        public int UserID
        {
            get { return (int)GetValue(UserIDProperty); }
            set { SetValue(UserIDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserIDProperty =
            DependencyProperty.Register("UserID", typeof(int), typeof(EmployeeUserWindow), new PropertyMetadata(0));


        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            Close();
            try
            {
                if (ID == 0)
                {
                    throw new Exception("You can't update a task that isn't exit!");
                }
                else
                {
                    BO.Task t = new BO.Task { Id = CurrentTask.Id, Alias = CurrentTask.Alias, Description = CurrentTask.Description, Status = CurrentTask.Status };
                    s_bl.Task.Update(t);
                    MessageBox.Show("The Task has been updated successfully", "message", MessageBoxButton.OK);
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EndTask(object sender, RoutedEventArgs e)
        {
            if (CurrentTask is not null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to end the task?", "message", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                s_bl.Task.EndTask(CurrentTask.Id, UserID);
            }
        }
        private void ChooseNewTask(object sender, RoutedEventArgs e)
        {
            if (CurrentTask is not null) 
            {
                MessageBoxResult result = MessageBox.Show("Do you want to choose a new task?", "message", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) 
                {
                    return;
                }
                s_bl.Task.EndTask(CurrentTask.Id, UserID);
                new TaskListForEmployeeWindow(UserID).Show();
            }
        }
    }
}