using PL.Employee;
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
using System.Xml.Linq;

namespace PL
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// bl is an instance of IBl
        /// </summary>
        BlApi.IBl bl = BlApi.Factory.Get();
        /// <summary>
        /// Constructor,Building an instance of LogInWindow
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
 //         MainWindow mw = new MainWindow();//create new OrderTrackingWindow
            Close();
//          mw.ShowDialog();
        }
        /// <summary>
        /// button to Log In as a new user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stack = (StackPanel)button.Parent;

            TextBox userNameTextBox = (TextBox)stack.Children[3];
            PasswordBox passwordTextBox = (PasswordBox)stack.Children[5];

            string userName = (string)userNameTextBox.Text;
            string password = (string)passwordTextBox.Password;
            string wrongInput = "";
            if (userName == "")
            {
                wrongInput += "UserName is Missing\n";
                userNameTextBox.BorderBrush = Brushes.Red;
            }
            
            if (password == "")
            {
                wrongInput += "Password is Missing\n";
                passwordTextBox.BorderBrush = Brushes.Red;
            }
            if (wrongInput != "")
            {
                MessageBox.Show(wrongInput, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                DO.User? user = bl.User.GetByUserName(userName);
                if (user != null)
                    if (user.Password != password)
                    {
                        MessageBox.Show("Passcode is wrong", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        passwordTextBox.BorderBrush = Brushes.Red;
                    }
                    else
                    {
                        if (user.IsManeger)//if it's an admin
                        {
                            ManagerWindow aw = new ManagerWindow();//create new ManagerWindow
                            Close();
                            aw.ShowDialog();
                        }
                        else //if it isn't an admin
                        {
                            EmployeeUserWindow cw = new EmployeeUserWindow(user.ID);//create new ManagerWindow
                            Close();
                            cw.ShowDialog();
                        }
                    }
            }
            catch (BO.BlDoesNotExistException)
            {
                MessageBox.Show("User name is wrong", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SignInButton(object sender, RoutedEventArgs e)
        {
            new SignUpWindow().ShowDialog();
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).BorderBrush == Brushes.Red)
            {
                ((TextBox)sender).BorderBrush = Brushes.DimGray;
            }
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (((PasswordBox)sender).BorderBrush == Brushes.Red)
            {
                ((PasswordBox)sender).BorderBrush = Brushes.DimGray;
            }
        }
    }
}
