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
        EmployeeList = s_bl?.Employee.ReadAll()!;
    }
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public IEnumerable<BO.EmployeeInTask> EmployeeList
    {
        get { return (IEnumerable<BO.EmployeeInTask>)GetValue(CourseListProperty); }
        set { SetValue(CourseListProperty, value); }
    }

    public static readonly DependencyProperty CourseListProperty =
        DependencyProperty.Register("CourseList", typeof(IEnumerable<BO.EmployeeInTask>), typeof(EmployeeListWindow), new PropertyMetadata(null));

}
