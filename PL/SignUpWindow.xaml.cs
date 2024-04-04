
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
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
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>

    public enum AdminAccess { Yes, No }
    public partial class SignUpWindow : Window
    {
        BlApi.IBl bl = BlApi.Factory.Get();
        
        /// <summary>
        /// Constructor,Building an instance of SignUpWindow
        /// </summary>
        /// <param name="adA"></param>
        public SignUpWindow()
        {
            InitializeComponent();
            
        }


        public bool ChBox
        {
            get { return (bool)GetValue(ChBoxProperty); }
            set { SetValue(ChBoxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChBoxProperty =
            DependencyProperty.Register("ChBox", typeof(bool), typeof(SignUpWindow), new PropertyMetadata(false));


        /// <summary>
        /// Button to Sign Up as an Admin or an employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stack = (StackPanel)button.Parent;

            TextBox userNameTextBox = (TextBox)stack.Children[3];
            TextBox userIdTextBox = (TextBox)stack.Children[5];
            PasswordBox passwordTextBox = (PasswordBox)stack.Children[7];
            string userName = (string)userNameTextBox.Text;
            string userIDstr = (string)userIdTextBox.Text;
            string password = (string)passwordTextBox.Password;
            bool isManager = ChBox;

            string wrongInput = "";

            if (userName == "")
            {
                wrongInput += "Name is missing\n";
                userNameTextBox.BorderBrush = Brushes.Red;
                //MessageBox.Show("Name is missing", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (userIDstr == "")
            {
                wrongInput += "UserID is Missing\n";
                userIdTextBox.BorderBrush = Brushes.Red;
            }
            if (password == "")
            {
                wrongInput += "Password is Missing\n";
                passwordTextBox.BorderBrush = Brushes.Red;
            }
            int userID;
            if (!int.TryParse(userIDstr, out userID))
            {
                wrongInput += "ID is not valid\n";
                passwordTextBox.BorderBrush = Brushes.Red;
            }
            if (wrongInput != "")
            {
                MessageBox.Show(wrongInput, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                BO.User user;
                user = new BO.User() { Name = userName, ID = userID, Password = password, IsManeger = isManager };
                bl.User.Create(user);
                MessageBox.Show("Signing up has ended successfully👌", "Good Luck", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            catch (BO.BlAlreadyExistsException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        //private void textChanged(object sender, RoutedEventArgs e)
        //{
        //    var tbPasscode = (sender as TextBox);
        //    if (tbPasscode!.BorderBrush == Brushes.Red)
        //    {
        //        tbPasscode.BorderBrush = Brushes.DimGray;
        //    }
        //}

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
            //MainWindow mw = new MainWindow();
            Close();
            //ShowDialog();
        }

     
    }
}
