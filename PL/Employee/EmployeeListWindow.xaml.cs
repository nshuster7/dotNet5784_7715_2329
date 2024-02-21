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
}
