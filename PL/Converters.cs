using BO;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL;

class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToBool : IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? true : false;
    }


    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class TaskStatusToVisibilityConverter : IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ProjectStatus projectStatus && parameter is string propertyName)
        {
            switch (projectStatus)
            {
                case ProjectStatus.PlanningStage:
                    if (propertyName == "ScheduledDate" || propertyName == "StartDate" || propertyName == "CompleteDate" || propertyName == "CreatedAtDate" || propertyName == "Employee")
                        return Visibility.Collapsed;
                    break;
                case ProjectStatus.IntermediateStage:
                    if (propertyName != "ScheduledDate")
                        return Visibility.Collapsed;
                    break;
                case ProjectStatus.ExecutionStage:
                    if (propertyName == "ScheduledDate" || propertyName == "StartDate" || propertyName == "CreatedAtDate" || propertyName == "Employee")
                        return Visibility.Collapsed;
                    break;
            }
        }

        return Visibility.Visible;
    }
    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class TaskStatusToBoolConverter : IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ProjectStatus projectStatus && parameter is string propertyName)
        {
            switch (projectStatus)
            {
                case ProjectStatus.PlanningStage:
                    if (propertyName == "ScheduledDate" || propertyName == "StartDate" || propertyName == "CompleteDate" || propertyName == "CreatedAtDate" || propertyName == "Employee")
                        return false;
                    break;
                case ProjectStatus.IntermediateStage:
                    if (propertyName != "ScheduledDate")
                        return false;
                    break;
                case ProjectStatus.ExecutionStage:
                    if (propertyName == "ScheduledDate" || propertyName == "StartDate" || propertyName == "CreatedAtDate" || propertyName == "Employee")
                        return false;
                    break;
            }
        }

        return true;
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}

//public class TaskStatusToBoolConverter : IValueConverter
//{
//    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
//    {
//        if ((BO.TaskStatus)value == BO.TaskStatus.OnTrack|| (BO.TaskStatus)value == BO.TaskStatus.Done) return false; return true;  
//    }

//    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }

//}
