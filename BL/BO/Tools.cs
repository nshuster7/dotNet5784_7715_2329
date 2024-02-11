using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BO;

public static class Tools
{
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
    public static int? GetCurrentTaskId(DalApi.IDal dal, int? idEmp)
    {
        DO.Task? t= dal.Task.Read(task => task.EmployeeId == idEmp);
        if (t != null)
           return t.Id;
        return null;
    }
    public static string? GetCurrentTaskAlias(DalApi.IDal dal, int? idEmp)
    {
        DO.Task? t = dal.Task.Read(task => task.EmployeeId == idEmp);
        if (t is not null)
            return t.Alias;
        return null;
    }
  
    public static TaskInEmployee? GetTaskInEmployee(DalApi.IDal dal, int? idEmp)
    {
        int? id = Tools.GetCurrentTaskId(dal, idEmp);
        if(id==null) return null;
         return new BO.TaskInEmployee {Id= (int)id, Alias= Tools.GetCurrentTaskAlias(dal, idEmp)};
    }
    public static EmployeeInTask? GetEmployeeInTask(DalApi.IDal dal, int idEmp)
    {
        DO.Employee? emp = dal.Employee.Read(idEmp);
        if(emp is not null)
           return new EmployeeInTask { Id=idEmp, Name=emp.Name };
        return null;
    }
    public static TaskStatus GetStatus(DO.Task doTask)
    {
        if (doTask == null)
        {
            throw new ArgumentNullException(nameof(doTask), "Task cannot be null");
        }

        // Determine the task status based on relevant properties
        if (doTask.CompleteDate.HasValue)
        {
            return TaskStatus.Done;
        }
        else if (doTask.ScheduledDate.HasValue)
        {
            if (DateTime.Now >= doTask.ScheduledDate)
            {
                return TaskStatus.OnTrack;
            }
            else
            {
                return TaskStatus.Scheduled;
            }
        }
        else
        {
            return TaskStatus.Unscheduled;
        }
    }
    public static bool CanTaskBeAssignedFor(DalApi.IDal dal,int id,int empId)
    {
        DO.Task? task =dal.Task.Read(id) ;
        TaskStatus s;
        if (task != null)
        {
            if(task.EmployeeId==empId)
                return true;
            s = GetStatus(task);
            if (s == TaskStatus.Unscheduled)
                return true;
            return false;
        }
        throw new BO.BlTaskCantBeAssignedException($"task with ID={id} cant be assigned for employee with ID={empId} ");
    }
    public static bool IsEmployeeWorkingOnTask(DalApi.IDal dal, int empId)
    {
        DO.Task? task = dal.Task.Read(empId);
        if (task == null) { return false; }
        TaskStatus status = GetStatus(task);
        if (status == TaskStatus.Done)
            return false;
        return true;
    }
    public static DateTime? GetMaxDate(DateTime? date1, DateTime? date2)
    {
        if (date1 == null && date2 == null)
            return DateTime.MinValue;
        return (date1 > date2) ? date1 : date2;
    }
    //The function receives an ID number of a task and checks whether the task is available
    //i.e. whether there is no one else working on the task and also whether all the tasks preceding it have been performed
    public static bool IsTaskAvailable(DalApi.IDal dal,DO.Task task)
    {
        if (task.EmployeeId != 0)
            return false;
        var dependencies = dal.Dependency.ReadAll().Where(d => d!.DependentTask == task.Id).ToList();// A list of dependencies when my task depends on others
        List<TaskInList> taskList = (from DO.Dependency d in dependencies
                                     let temporary = dal.Task.Read(d.DependsOnTask ?? 0) //A list of tasks that the current task depends on
                                     select new TaskInList
                                     {
                                         Id = d.DependsOnTask ?? 0,
                                         Description = temporary.Description,
                                         Alias = temporary.Alias,
                                         Status = Tools.GetStatus(temporary)
                                     }).ToList();
        foreach (var prevTask in taskList)
        {
            if (prevTask.Status != TaskStatus.Done)
                return false;
        }
        return true;
    }
     public static List<TaskInList> GetListOfPreviousTask(DalApi.IDal dal, int id)
    {
        List<TaskInList> taskList = (from DO.Dependency d in dal.Dependency.ReadAll()
                                     where d.DependentTask == id
                                     let temporary = dal.Task.Read(d.DependsOnTask ?? 0)
                                     select new TaskInList
                                     {
                                         Id = d.DependsOnTask ?? 0,
                                         Description = temporary.Description,
                                         Alias = temporary.Alias,
                                         Status = Tools.GetStatus(temporary)
                                     }).ToList();
        return taskList;
    }
    public static BO.Type? GetComplexity(DalApi.IDal dal, DO.Task task)
    {
        int? temp = (int?)task.Complexity;
        BO.Type? complexity = null;
        if (temp != null)
            return complexity = (BO.Type)temp;
        return null;
    }
    public static bool CanTaskBeDeleted(DalApi.IDal dal, DO.Task task)
    {
        DO.Task? task1=dal.Task.Read(task.Id);
        if(task1 == null) return false;
        var dep = dal.Dependency.ReadAll(d => d.DependsOnTask == task.Id);
        if (dep != null) return false;
        return true;                           
    }

    public static List<EmployeeInTask> GetSortedEmployees(DalApi.IDal dal)
    {
        IEnumerable<DO.Employee?> employees = dal.Employee.ReadAll();
        List<EmployeeInTask> employeeList = employees.Select(emp => new EmployeeInTask()
        {
            Id = emp.Id,
            Name = emp.Name
        }).OrderBy(emp => emp.Name).ToList();
        return employeeList;
    }
}



