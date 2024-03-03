using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        private BlApi.IBl s_bl = BlApi.Factory.Get();

        User user = new User();
        public CreateUserWindow(bool isManeger)
        {
            InitializeComponent();

            general.DataContext = user;
            user.IsManeger = isManeger;
        }

        // makes sure the user can enter only numbers
        private void allowOnlyNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //if (txbID.Text.Length == 0 || txbName.Text.Length == 0 || txbName.Text.StartsWith(' ') == true || txbPassword.Text.Length == 0)
            //    MessageBox.Show("Fill in all the details!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //else
            {
                try
                {
                    s_bl.User.Create(user);
                    MessageBox.Show("User has added", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch (Exception except)
                {
                    MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // prevent white spaces
        private void preventWhiteSpaces(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }
    }
}
