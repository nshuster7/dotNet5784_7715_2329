using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Employee;

/// <summary>
/// Interaction logic for EmployeeListWindow.xaml
/// </summary>
public partial class EmployeeListWindow : Window
{
    public EmployeeListWindow()
    {
        InitializeComponent();
        EmployeeList = s_bl.Employee.ReadAll();
    }
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public IEnumerable<BO.Employee> EmployeeList
    {
        get { return (IEnumerable<BO.Employee>)GetValue(EmployeeListProperty); }
        set { SetValue(EmployeeListProperty, value); }
    }

    public static readonly DependencyProperty EmployeeListProperty =
        DependencyProperty.Register("EmployeeList", typeof(IEnumerable<BO.Employee>), typeof(EmployeeListWindow), new PropertyMetadata(null));

    public BO.Type Type { get; set; } = BO.Type.All;

    private void WorkerListSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EmployeeList = (Type == BO.Type.All) ?
            s_bl?.Employee.ReadAll()! : s_bl?.Employee.ReadAll(item => (BO.Type)item.Type! == Type)!;
    }

    private void AddEmployee(object sender, RoutedEventArgs e)
    {
        new EmployeeWindow().ShowDialog();
        EmployeeList = (Type == BO.Type.All) ?
        s_bl?.Employee.ReadAll()! : s_bl?.Employee.ReadAll(item => (BO.Type)item.Type! == Type)!;
    }

    private void UpdateEmployee(object sender, MouseButtonEventArgs e)
    {
        BO.Employee? employee = (sender as ListView)?.SelectedItem as BO.Employee;
       
        if (employee != null)
        {
            new EmployeeWindow(employee.Id).ShowDialog();
            //update the list of the workers after the changes
            EmployeeList = (Type == BO.Type.All) ?
            s_bl?.Employee.ReadAll()! : s_bl?.Employee.ReadAll(item => (int)item.Type! == (int)Type)!;
        }
    }   
}
