﻿using BO;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

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
class TaskStatusToBoolConverter : IValueConverter
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

class ConvertStatusToBackground : IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        switch (value)
        {
            case "Unscheduled":
                return Brushes.Gray;
            case "Scheduled":
                return Brushes.PowderBlue;
            case "OnTrack":
                return Brushes.CornflowerBlue;
            case "InJeopardy":
                return Brushes.MediumVioletRed;
            case "Done":
                return Brushes.Blue;
            default:
                return Brushes.White;
        }
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStatusToForeground : IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        switch (value)
        {
            case "Unscheduled":
                return Brushes.Gray;
            case "Scheduled":
                return Brushes.PowderBlue;
            case "OnTrack":
                return Brushes.CornflowerBlue;
            case "InJeopardy":
                return Brushes.MediumVioletRed;
            case "Done":
                return Brushes.Blue;
            case "All":
                return Brushes.White;
            default:
                return Brushes.Black;
        }
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

