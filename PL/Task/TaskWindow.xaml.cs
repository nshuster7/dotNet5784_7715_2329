using System.Windows;
namespace PL.Task
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        int ID;
        public TaskWindow(int Id1 = 0)
        {
            InitializeComponent();
            ID = Id1;
            if (Id1 == 0)
            {
                CurrentTask = new BO.Task { Id = 0 };
            }
            else
            {
                try
                {
                    CurrentTask = s_bl.Task.Read(Id1)!;
                }
                catch (Exception except)
                {
                    MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        public void UpdateAddClick(object sender, RoutedEventArgs e)
        {
            Close();
            try
            {
                if (ID == 0)
                {
                    s_bl.Task.Create(CurrentTask);
                    MessageBox.Show("The Task has been added successfully", "message", MessageBoxButton.OK);
                }
                else
                {
                    s_bl.Task.Update(CurrentTask);
                    MessageBox.Show("The Task has been updated successfully", "message", MessageBoxButton.OK);
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}