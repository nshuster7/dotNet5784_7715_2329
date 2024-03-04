using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Label.SetBinding(ContentProperty, new Binding("CurrentTime"));
        }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register(
            "CurrentTime",
            typeof(DateTime),
            typeof(MainWindow),
            new PropertyMetadata(DateTime.Now));

        public DateTime CurrentTime
        {
            get => (DateTime)GetValue(CurrentTimeProperty);
            set => SetValue(CurrentTimeProperty, value);
        }

      
        private void AdvanceTimeByMonth(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByMonth();
            CurrentTime = new DateTime(s_bl.Clock.Year, s_bl.Clock.Month, s_bl.Clock.Day, CurrentTime.Hour, CurrentTime.Minute, CurrentTime.Second);
        }


        private void AdvanceTimeByDay(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByDay();
            CurrentTime = new DateTime(s_bl.Clock.Year, s_bl.Clock.Month, s_bl.Clock.Day, CurrentTime.Hour, CurrentTime.Minute, CurrentTime.Second);
        }

        private void AdvanceTimeByYear(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByYear();
            CurrentTime = new DateTime(s_bl.Clock.Year, s_bl.Clock.Month, s_bl.Clock.Day, CurrentTime.Hour, CurrentTime.Minute, CurrentTime.Second);
        }

        private void AdvanceTimeByHour(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByHour();
            CurrentTime = s_bl.Clock;
        }
    }
}
