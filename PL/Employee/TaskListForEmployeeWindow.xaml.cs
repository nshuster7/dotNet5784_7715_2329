using BO;
using PL.Task;
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

namespace PL.Employee
{
    /// <summary>
    /// Interaction logic for TaskListForEmployeeWindow.xaml
    /// </summary>
    public partial class TaskListForEmployeeWindow : Window
    {
        public TaskListForEmployeeWindow(int id)
        {
            InitializeComponent();
            TaskList = s_bl.Task.TasksForWorker(id);
            ID = id;
        }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof(int), typeof(TaskListForEmployeeWindow), new PropertyMetadata(0));


        public IEnumerable<BO.TaskInList>? TaskList
        {
            get { return (IEnumerable<BO.TaskInList>?)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListForEmployeeWindow), new PropertyMetadata(null));

        private void StartTask(object sender, RoutedEventArgs e)
        {
            BO.TaskInList? taskInList = (sender as ListView)?.SelectedItem as BO.TaskInList;
           
            try
            {
                if (taskInList != null)
                    s_bl.Task.StartTask(taskInList.Id, ID);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }








}
