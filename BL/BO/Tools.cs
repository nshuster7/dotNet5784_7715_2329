using DalApi;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BO;

public static class Tools
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    public static string ToStringProperty<T>(this T obj)
    {
        StringBuilder sb = new StringBuilder();

        // Get all properties of the object
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            // Get the property value
            object? value = property.GetValue(obj);

            // Check if the property is a collection
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                // Iterate over the collection and add each item to the string builder
                foreach (object item in (IEnumerable<object>)value!)
                {
                    sb.AppendLine($"{property.Name}: {item}");
                }
            }
            else
            {
                // Add the property value to the string builder
                sb.AppendLine($"{property.Name}: {value}");
            }
        }

        return sb.ToString();
    }
     public static bool IsValidEmail(string? email)
    {
        if (email != null)
        {
            // Define a regular expression pattern for a basic email validation
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }
        return true;
    }
    public static DateTime? GetMaxDate(DateTime? date1, DateTime? date2)
    {
        if (date1 == null && date2 == null)
            return DateTime.MinValue;
        return (date1 > date2) ? date1 : date2;
    }
    public static int? GetCurrentTaskId(this int? idEmp)
    {
        DO.Task? t = _dal.Task.Read(task => task.EmployeeId == idEmp);
        if (t != null)
            return t.Id;
        return null;
    }
    public static bool CanTaskBeAssignedFor(int id, int empId)
    {
        DO.Task? task = _dal.Task.Read(id);
        BO.TaskStatus s;
        if (task != null)
        {
            if (task.EmployeeId == empId)
                return true;
            s = GetStatus(task);
            if (s == BO.TaskStatus.Unscheduled)
                return true;
            return false;
        }
        throw new BO.BlTaskCantBeAssignedException($"task with ID={id} cant be assigned for employee with ID={empId} ");
    }
    public static BO.TaskStatus GetStatus(DO.Task doTask)
    {
        if (doTask == null)
        {
            throw new ArgumentNullException(nameof(doTask), "Task cannot be null");
        }

        // Determine the task status based on relevant properties
        if (doTask.CompleteDate.HasValue)
        {
            return BO.TaskStatus.Done;
        }
        else if (doTask.ScheduledDate.HasValue)
        {
            if (DateTime.Now >= doTask.ScheduledDate)
            {
                return BO.TaskStatus.OnTrack;
            }
            else
            {
                return BO.TaskStatus.Scheduled;
            }
        }
        else
        {
            return BO.TaskStatus.Unscheduled;
        }
    }
    public static bool IsEmployeeWorkingOnTask(int empId)
    {
        DO.Task? task = _dal.Task.Read(empId);
        if (task == null) { return false; }
        BO.TaskStatus status = Tools.GetStatus(task);
        if (status == BO.TaskStatus.Done)
            return false;
        return true;
    }
    public static void clear()
    {
        _dal.Employee.Clear();
        _dal.Task.Clear();
        _dal.Dependency.Clear();
    }

}



