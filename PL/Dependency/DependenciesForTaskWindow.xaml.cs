using BO;
using PL.Task;
using System;
using System.Collections.Generic;

using System.Collections.Immutable;
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

namespace PL.Dependency
{
    /// <summary>
    /// Interaction logic for DependenciesForTaskWindow.xaml
    /// </summary>
    public partial class DependenciesForTaskWindow : Window
    {
        public DependenciesForTaskWindow()
        {
            InitializeComponent();

            SelectedTask = s_bl.Task.Read(CurrentTask.Id)!;
            //List<TaskInList>? tasks = SelectedTask.Dependencies;
            DependOnTasks = SelectedTask.Dependencies?.ToImmutableArray() ?? ImmutableArray<TaskInList>.Empty;

        }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Task SelectedTask
        {
            get { return (BO.Task)GetValue(SelectedTaskProperty); }
            set { SetValue(SelectedTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTaskProperty =
            DependencyProperty.Register("SelectedTask", typeof(BO.Task), typeof(DependenciesForTaskWindow), new PropertyMetadata(null));


        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(DependenciesForTaskWindow), new PropertyMetadata(null));

        public BO.Type Type { get; set; } = BO.Type.All;




        public ImmutableArray<TaskInList> DependOnTasks
        {
            get { return (ImmutableArray<TaskInList>)GetValue(DependOnTasksProperty); }
            set { SetValue(DependOnTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DependOnTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DependOnTasksProperty =
            DependencyProperty.Register("DependOnTasks", typeof(ImmutableArray<TaskInList>), typeof(DependenciesForTaskWindow), new PropertyMetadata(null));


    }
}
