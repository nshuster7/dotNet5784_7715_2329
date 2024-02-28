using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    public TaskListWindow()
    {
        InitializeComponent();
        TaskList = s_bl.Task.ReadAllTaskInList();
    }
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

    public BO.Type Type { get; set; } = BO.Type.All;

    private void TaskListSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TaskList = (Type == BO.Type.All) ?
            s_bl?.Task.ReadAllTaskInList()! : s_bl?.Task.ReadAllTaskInList(item => (BO.Type)item.Complexity! == Type)!;
    }

    private void AddTask(object sender, RoutedEventArgs e)
    {
        new TaskWindow().ShowDialog();
        TaskList = (Type == BO.Type.All) ?
        s_bl?.Task.ReadAllTaskInList()! : s_bl?.Task.ReadAllTaskInList(item => (BO.Type)item.Complexity! == Type)!;
    }

    private void UpdateTask(object sender, MouseButtonEventArgs e)
    {
        BO.TaskInList? Task = (sender as ListView)?.SelectedItem as BO.TaskInList;

        if (Task != null)
        {
            new TaskWindow(Task.Id).ShowDialog();
            //update the list of the workers after the changes
            TaskList = (Type == BO.Type.All) ?
            s_bl?.Task.ReadAllTaskInList()! : s_bl?.Task.ReadAllTaskInList(item => (BO.Type)item.Complexity! == Type)!;
        }
    }
}
