﻿using System.Windows;
namespace PL.Employee
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        int ID;
        public EmployeeWindow(int Id1=0)
        {
            InitializeComponent();
            ID = Id1;
            if(Id1 == 0)
            {
                CurrentEmployee = new BO.Employee{Id = 0};
            }
            else 
            {
                try
                {
                    CurrentEmployee = s_bl.Employee.Read(Id1)!;
                    Image = CurrentEmployee.ImageRelativeName;
                }
                catch (Exception except)
                {
                    MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        public string? Image
        {
            get { return (string?)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(EmployeeWindow), new PropertyMetadata(" "));


        public BO.Employee CurrentEmployee
        {
            get { return (BO.Employee)GetValue(EmployeeProperty); }
            set { SetValue(EmployeeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmployeeProperty =
            DependencyProperty.Register("CurrentEmployee", typeof(BO.Employee), typeof(EmployeeWindow), new PropertyMetadata(null));

        public void UpdateAddClick(object sender, RoutedEventArgs e)
        {
            Close();
            try
            {
                if (ID==0)
                {
                    s_bl.Employee.Create(CurrentEmployee);
                    MessageBox.Show("The Employee has been added successfully", "message", MessageBoxButton.OK);
                }
                else
                {
                    s_bl.Employee.Update(CurrentEmployee);
                    MessageBox.Show("The employee has been updated successfully", "message", MessageBoxButton.OK);
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void AddAndUpdateEmployee(object sender, MouseButtonEventArgs e)
        //{
        //    if ((sender as Button)!.Content.ToString() == "Add")
        //    {
        //        try
        //        {
        //            int? id = s_bl.Employee.Create(CurrentEmployee);
        //            MessageBox.Show($"worker {id} was successfuly added!", "Success", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //            Close();
        //        }
        //        catch (BlAlreadyExistsException excpt)
        //        {
        //            MessageBox.Show(excpt.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        }
        //        catch (Exception excpt)
        //        {
        //            MessageBox.Show(excpt.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        }
        //    }
        //    else
        //        try
        //        {
        //            s_bl.Employee.Update(CurrentEmployee!);
        //            MessageBox.Show($"worker {CurrentEmployee.Id} was successfuly added!", "Success", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //            Close();
        //        }
        //        catch (BlAlreadyExistsException excpt)
        //        {
        //            MessageBox.Show(excpt.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        }
        //        catch (Exception excpt)
        //        {
        //            MessageBox.Show(excpt.Message, "Eror", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        }
        //}
    }
}
