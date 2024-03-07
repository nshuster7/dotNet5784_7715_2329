using BlApi;
using PL.Dependency;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for EmployeeUserWindow.xaml
    /// </summary>
    public partial class EmployeeUserWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        int ID;
        public EmployeeUserWindow(int Id1 = 0)
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
                    CurrentTask = s_bl.Task.ReadAll().FirstOrDefault(t => t.Id == Id1)!;
                }
                catch (Exception except)
                {
                    MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            ProjectStatus = IBl.GetProjectStatus();
        }
        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(EmployeeUserWindow), new PropertyMetadata(null));


        public BO.ProjectStatus ProjectStatus
        {
            get { return (BO.ProjectStatus)GetValue(ProjectStatusProperty); }
            set { SetValue(ProjectStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectStatusProperty =
            DependencyProperty.Register("ProjectStatus", typeof(BO.ProjectStatus), typeof(EmployeeUserWindow), new PropertyMetadata(null));


        public void UpdateAddClick(object sender, RoutedEventArgs e)
        {
            Close();
            try
            {
                if (ID == 0)
                {
                    BO.Task t = new BO.Task { Id = CurrentTask.Id, Alias = CurrentTask.Alias, Description = CurrentTask.Description, Status = CurrentTask.Status };
                    s_bl.Task.Create(t);
                    MessageBox.Show("The Task has been added successfully", "message", MessageBoxButton.OK);
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

        }

        private void ChooseNewTask(object sender, RoutedEventArgs e)
        {

        }
    }
}
