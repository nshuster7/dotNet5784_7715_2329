using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BO;

public static class Tools
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    //public static string ToStringProperty<T>(this T obj)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    // Get all properties of the object
    //    PropertyInfo[] properties = typeof(T).GetProperties();
    //    foreach (PropertyInfo property in properties)
    //    {
    //        // Get the property value
    //        object? value = property.GetValue(obj);
    //        // Check if the property is a collection
    //        if (property.PropertyType.IsGenericType &&
    //            property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
    //        {
    //            // Iterate over the collection and add each item to the string builder
    //            foreach (object item in (IEnumerable<object>)value!)
    //            {
    //                sb.AppendLine($"{property.Name}: {item}");
    //            }
    //        }
    //        else
    //        {
    //            // Add the property value to the string builder
    //            sb.AppendLine($"{property.Name}: {value}");
    //        }
    //    }
    //    return sb.ToString();
    //}
    public static string ToStringProperty<T>(this T t) where T : struct => 
        t.GetType().GetProperties().Aggregate("", (str, prop) => str + "\n" + prop.Name + ": " + prop.GetValue(t, null));
    /// <summary>
    /// Checks if a string represents a valid email address.
    /// </summary>
    /// <param name="email">The string representing the email address to check.</param>
    /// <returns>
    /// True if the string represents a valid email address, false otherwise.
    /// Note that the function returns true for null input as well.
    /// </returns>
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
    /// <summary>
   /// 
   /// </summary>
   /// <param name="date1"></param>
   /// <param name="date2"></param>
   /// <returns></returns>
    public static DateTime? GetMaxDate(DateTime? date1, DateTime? date2)
    {
        if (date1 == null && date2 == null)
            return DateTime.MinValue;
        return (date1 > date2) ? date1 : date2;
    }
    /// <summary>
    /// Returns the ID of the current task for the employee with the given ID, or null if there is no current task.
    /// </summary>
    /// <param name="idEmp">The ID of the employee.</param>
    /// <returns>The ID of the current task for the employee, or null if there is no current task.</returns>
    public static int? GetCurrentTaskId(this int? idEmp)
    {
        DO.Task? t = _dal.Task.Read(task => task.EmployeeId == idEmp);
        if (t != null)
            return t.Id;
        return null;
    }
    /// <summary>
    /// Checks if a specific task can be assigned to a specific employee.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="empId">The ID of the employee.</param>
    /// <returns>
    /// True if the task can be assigned to the employee, false otherwise.
    /// Throws a BlTaskCantBeAssignedException in the following cases:
    ///   - The task with the given ID does not exist.
    /// </returns>
    public static bool CanTaskBeAssignedFor(int id, int empId)
    {
        DO.Task? task = _dal.Task.Read(id);
        BO.TaskStatus s;
        if (task != null)
        {
            if (task.EmployeeId == empId) // Task already assigned to the same employee
                return true;
            s = GetStatus(task);
            if (s == BO.TaskStatus.Unscheduled)
                return true;
            return false;// Task is scheduled for a different employee
        }
        throw new BO.BlTaskCantBeAssignedException($"task with ID={id} cant be assigned for employee with ID={empId} ");
    }
    /// <summary>
    /// Gets the current status of a task.
    /// </summary>
    /// <param name="doTask">A DO.Task object representing the task.</param>
    /// <returns>
    /// The current status of the task - Done, OnTrack, Scheduled, Unscheduled.
    /// Throws an ArgumentNullException if the doTask object is null.
    /// </returns>
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
    /// <summary>
    /// Checks if an employee with the given ID is currently working on any task.
    /// </summary>
    /// <param name="empId">The ID of the employee.</param>
    /// <returns>
    /// True if the employee is working on any task, false otherwise.
    /// </returns>
    public static bool IsEmployeeWorkingOnTask(int empId)
    {
        DO.Task? task = _dal.Task.Read(empId);
        if (task == null) { return false; }
        BO.TaskStatus status = Tools.GetStatus(task);
        if (status == BO.TaskStatus.Done)
            return false;
        return true;
    }
    /// <summary>
    /// Clears all data from all repositories in the system.
    /// </summary>
    public static void clear()
    {
        _dal.Employee.Clear();
        _dal.Task.Clear();
        _dal.Dependency.Clear();
    }

}



