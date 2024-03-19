
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
        AdminAccess adminAccess; //for admin access or customer
        /// <summary>
        /// Constructor,Building an instance of SignUpWindow
        /// </summary>
        /// <param name="adA"></param>
        public SignUpWindow(AdminAccess adA)
        {
            InitializeComponent();
            adminAccess = adA;
        }
        /// <summary>
        /// Button to Sign Up as an Admin or a customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stack = (StackPanel)button.Parent;

            TextBox userNameTextBox = (TextBox)stack.Children[3];
            PasswordBox passwordTextBox = (PasswordBox)stack.Children[5];
            //string wrongInput = "";
            //string name = tbName.Text;
            //string userEmail = tbUserEmail.Text;
            //string userAddress = tbUserAddress.Text;
            //string userName = tbUserName.Text;
            //string passcode = tbPasscode.Password;
            //if (name == "")
            //{
            //    wrongInput += "Name is missing\n";
            //    tbName.BorderBrush = Brushes.Red;
            //    //MessageBox.Show("Name is missing", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //bool isValidEmail = true;
            //try
            //{
            //    var mail = new MailAddress(userEmail);
            //    isValidEmail = mail.Host.Contains(".");
            //    if (!isValidEmail)//checks if the mail is valid
            //    {
            //        wrongInput += "UserEmail is invalid\n";
            //        tbUserEmail.BorderBrush = Brushes.Red;
            //    }
            //}
            //catch (Exception)
            //{
            //    wrongInput += "UserEmail is invalid\n";
            //    tbUserEmail.BorderBrush = Brushes.Red;
            //}
            //if (userEmail == "")
            //{
            //    wrongInput += "UserEmail is Missing\n";
            //    tbUserEmail.BorderBrush = Brushes.Red;
            //}


            //if (userAddress == "")
            //{
            //    wrongInput += "UserAddress is Missing\n";
            //    tbUserAddress.BorderBrush = Brushes.Red;
            //}
            //if (userName == "")
            //{
            //    wrongInput += "UserName is Missing\n";
            //    tbUserName.BorderBrush = Brushes.Red;
            //}
            //if (passcode == "")
            //{
            //    wrongInput += "Password is Missing\n";
            //    tbPasscode.BorderBrush = Brushes.Red;
            //}
            //if (wrongInput != "")
            //{
            //    MessageBox.Show(wrongInput, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            //try
            //{
            //    BO.User user;
            //    if (adminAccess == AdminAccess.No) //if itsn't an admin
            //    {
            //        user = new BO.User() { Name = name, UserName = userName, UserEmail = userEmail, UserAddress = userAddress, Passcode = passcode, AdminAccess = false };
            //        bl.User.Add(user);
            //        MessageBox.Show("Signing up has ended successfully👌", "Good Luck", MessageBoxButton.OK, MessageBoxImage.Information);

            //        BO.Cart cart = new BO.Cart() { CustomerAddress = user.UserAddress, CustomerEmail = user.UserEmail, CustomerName = user.Name, Items = new List<BO.OrderItem>() };
            //        CatalogWindow cw = new CatalogWindow(cart);//create new ProductListWindow
            //        Close();
            //        cw.ShowDialog();
            //    }
            //    else //if it is an admin
            //    {
            //        user = new BO.User() { Name = name, UserName = userName, UserEmail = userEmail, UserAddress = userAddress, Passcode = passcode, AdminAccess = true };
            //        bl.User.Add(user);
            //        MessageBox.Show("Signing up has ended successfully👌", "Good Luck", MessageBoxButton.OK, MessageBoxImage.Information);
            //        ManagerWindow aw = new ManagerWindow(); //create new Admin Window
            //        Close();
            //        aw.ShowDialog();

            //    }

            //}
            //catch (BO.BlAlreadyExistsException ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //}


        }

        private void textChanged(object sender, RoutedEventArgs e)
        {
            if (tbPasscode.BorderBrush == Brushes.Red)
            {
                tbPasscode.BorderBrush = Brushes.DimGray;
            }
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
            //MainWindow mw = new MainWindow();
            Close();
            //ShowDialog();
        }


    }
}
