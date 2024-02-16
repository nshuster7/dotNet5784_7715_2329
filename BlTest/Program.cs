using BO;

using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace BlTest;

partial class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
        {
            Tools.clear();
            DalTest.Initialization.Do();
        }
        try
        {
            BO.Entity choice;
        do
        {
            Console.WriteLine("For Employee Entity press: 1");
            Console.WriteLine("For Task Entity press: 2");
            Console.WriteLine("To create a schedule press: 3");
            Console.WriteLine("For exit press: 0");

            
                
                if (!Enum.TryParse(Console.ReadLine(), out choice))
                    throw new BlWrongValueException("WORNG VALUE");
                switch (choice)
                {
                    case BO.Entity.Employee:
                        employeeFunc();
                        break;
                    case Entity.Task:
                        taskFunc();
                        break;
                    case Entity.Schedule:
                       // Console.WriteLine("To create an automatic schedule press: 1");
                        Console.WriteLine("To create a manual schedule press: 2");
                        if (!int.TryParse(Console.ReadLine(), out int scheduleChoice))
                            throw new BlWrongValueException("WORNG VALUE");
                        //if (scheduleChoice == 1)
                        //    s_bl.AutomaticSchedule();
                        //else 
                        if (scheduleChoice == 2)
                            s_bl.ManualSchedule();
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid value Please try again");
                        break;
                }
        }
        while(choice != BO.Entity.Exit);
        }
            catch (Exception ex)
            {
            Console.WriteLine(ex.Message);
        }
    }


    static void employeeFunc()
    {
        Console.WriteLine("For exit press: 0");
        Console.WriteLine("To add a new employee press: 1");
        Console.WriteLine("To display employee by ID press: 2");
        Console.WriteLine("To display a list of the employee press: 3");
        Console.WriteLine("To update employee press: 4");
        Console.WriteLine("To delete employee from the list press: 5");
        Console.WriteLine("To start task press: 6");
        Console.WriteLine("To end task press: 7");
        Console.WriteLine("To sign up for a task press: 8");

        MenuEmployee choice;
        if (!Enum.TryParse(Console.ReadLine(), out choice))
            throw new BlWrongValueException("WORNG VALUE");

        switch (choice)
        {
            case MenuEmployee.Add:
                CreateE();
                break;
            case MenuEmployee.Print:
                ReadE();
                break;
            case MenuEmployee.PrintListOfEmployees:
                ReadAllE();
                break;
            case MenuEmployee.Update:
                UpdateE();
                break;
            case MenuEmployee.Delete:
                DeleteE();
                break;
            case MenuEmployee.StartTask:
                StartTask();
                break;
            case MenuEmployee.EndTask:
                EndTask();
                break;
            case MenuEmployee.SignForTask:
                SignUpForTask();
                break;
            case MenuEmployee.Exit:
                return;
            default:
                Console.WriteLine("Invalid value Please try again");
                break;
        }
    }

    static void taskFunc()
    {
        Console.WriteLine("For exit press: 0");
        Console.WriteLine("To add a new task press: 1");
        Console.WriteLine("To display task by ID press: 2");
        Console.WriteLine("To display a list of the task press: 3");
        Console.WriteLine("To update task press: 4");
        Console.WriteLine("To delete task from the list press: 5");
        Console.WriteLine("To display the list of the task the employee can chose press: 6");


        MenuTask choice;
        if (!Enum.TryParse(Console.ReadLine(), out choice))
            throw new BlWrongValueException("WORNG VALUE");

        switch (choice)
        {
            case MenuTask.Add:
                CreateT();
                break;
            case MenuTask.Print:
                ReadT();
                break;
            case MenuTask.PrintListOfTasks:
                ReadAllT();
                break;
            case MenuTask.Update:
                UpdateT();
                break;
            case MenuTask.Delete:
                DeleteT();
                break;
            case MenuTask.ListOfTasksThatEmployeeCanDo:
                ListOfTasksForWorker();
                break;
            case MenuTask.Exit:
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid value Please try again");
                break;
        }
    }



    static void CreateE()
    {
        Console.WriteLine("Enter ID, the worker's level, cost per hour, email and name:");

        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWrongValueException("WORNG ID");
        if (!int.TryParse(Console.ReadLine(), out int level))
            throw new BlWrongValueException("WORNG LEVEL");
        if (!int.TryParse(Console.ReadLine(), out int hourlyRate))
            throw new BlWrongValueException("WORNG COST");
        string email = Console.ReadLine()!;
        string name = Console.ReadLine()!;

        BO.Employee employee = new BO.Employee { Id = id, Name = name ,Email = email, Status= WorkStatus.Active, Type = (BO.Type)level, HourlyRate = hourlyRate, CurrentTaskId = null };
        Console.WriteLine(s_bl.Employee.Create(employee));
        return;
    }

    static void ReadE()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWrongValueException("WORNG ID");
        Console.WriteLine(s_bl.Employee.Read(id));
        return;
    }

    static void ReadAllE()
    {
        List<BO.EmployeeInTask> list;
        list = s_bl.Employee.ReadAll().ToList();
        foreach (BO.EmployeeInTask? worker in list)
            Console.WriteLine(worker);
    }

    static void UpdateE()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
            throw new BlWrongValueException("WRONG ID");
        BO.Employee employee = s_bl.Employee.Read(employeeId)!;
        Console.WriteLine(employee);
        int id = employee.Id;
        BO.Type ?level = employee.Type;
        BO.WorkStatus? status=employee.Status;
        string email = employee.Email?? "0@gmail.com";
        int cost = employee.HourlyRate;
        string name = employee.Name??" ";
        BO.TaskInEmployee? currentTask = employee.CurrentTaskId;

        Console.WriteLine("If you want to change the level of the worker enter the new level, else press -1");
        if (!int.TryParse(Console.ReadLine(), out int newLevel))
            throw new BlWrongValueException("WORNG LEVEL");
        if (newLevel != -1)
            level = (BO.Type)newLevel;

        Console.WriteLine("If you want to change the email enter the new email, else enter no");
        string newEmail = Console.ReadLine()!;
        if (newEmail != "no")
            email = newEmail;
        Console.WriteLine("If you want to change the status of the worker enter the new status, else press -1");
        if (!int.TryParse(Console.ReadLine(), out int newStatus))
            throw new BlWrongValueException("WORNG STATUS");
        if (newStatus != -1)
            status = (BO.WorkStatus)newStatus;

        Console.WriteLine("If you want to change the hourly rate enter the new hourly rate, else press -1");
        if (!int.TryParse(Console.ReadLine(), out int newHourlyRate))
            throw new BlWrongValueException("WORNG Hourly rate");
        if (newHourlyRate != -1)
            cost = newHourlyRate;

        Console.WriteLine("If you want to change the name enter the new name, else enter no");
        string newName = Console.ReadLine()!;
        if (newName != "no")
            name = newName;

        BO.Employee workerToUpdate = new BO.Employee { Id = id, Name = name, Email = email,Status= status, Type = level, HourlyRate = cost,  CurrentTaskId = currentTask };
        s_bl.Employee.Update(workerToUpdate);
    }

    static void DeleteE()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWrongValueException("WORNG ID");
        s_bl.Employee.Delete(id);
    }

    static void StartTask()
    {
        Console.WriteLine("Enter the ID of the employee:");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
            throw new BlWrongValueException("WORNG ID");
        Console.WriteLine("Enter the ID of the task you want to start:");
        if (!int.TryParse(Console.ReadLine(), out int taskId))
            throw new BlWrongValueException("WORNG ID");
        BO.Task? task = s_bl.Task.Read(taskId);
        if (task != null)
            s_bl.Task.StartTask(taskId, employeeId);
    }

    static void EndTask()
    {
        Console.WriteLine("Enter the ID of the employee:");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
            throw new BlWrongValueException("WORNG ID");
        Console.WriteLine("Enter the ID of the task you want to finish:");
        if (!int.TryParse(Console.ReadLine(), out int taskId))
            throw new BlWrongValueException("WORNG ID");
        s_bl.Task.EndTask(taskId, employeeId);
    }

    static void SignUpForTask()
    {
        Console.WriteLine("Enter the ID of the employee:");
        if (!int.TryParse(Console.ReadLine(), out int workerId))
            throw new BlWrongValueException("WORNG ID");
        Console.WriteLine("Enter the ID of the task you want to sign up to:");
        if (!int.TryParse(Console.ReadLine(), out int taskId))
            throw new BlWrongValueException("WORNG ID");
        s_bl.Task.SignUpForTask(taskId, workerId);
    }

    static void CreateT()
    {
        Console.WriteLine("Enter alias, description, complexity, deliverables, remarks");

        string alias = Console.ReadLine()!;
        string description = Console.ReadLine()??" ";
        DateTime createdAtDate = DateTime.Today;
        if (!int.TryParse(Console.ReadLine(), out int complexity))
            throw new BlWrongValueException("WORNG COMPLEXITY");
        string deliverables = Console.ReadLine()??" ";
        string remarks = Console.ReadLine() ?? " ";
        Console.WriteLine("Enter the time needed to complete the task");
        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan requiredEffortTime))
            throw new BlWrongValueException("the time is not correct");
        List<BO.TaskInList>? list = new();
        Console.WriteLine("If this task depends on other tasks enter 'yes' otherwise enter 'no':");
        string answer = Console.ReadLine()!;
        if (answer == "yes")
        {
            IEnumerable<BO.TaskInList> tasks = s_bl.Task.ReadAllTaskInList();
            do
            {
                Console.WriteLine($"Enter the ID of the task on which the task depends:");
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out int id))
                        throw new BlWrongValueException("WORNG ID");
                    BO.TaskInList? taskInList = tasks.FirstOrDefault(task => task.Id == id);
                    if (taskInList != null)
                        list.Add(taskInList);
                    else
                        throw new BlDoesNotExistException($"Task with ID={id} doe's NOT exists");
                }
                catch (BlDoesNotExistException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("If you want to add more dependency enter 'yes' otherwise enter 'no':");
                answer = Console.ReadLine()!;
            }
            while (answer == "yes");
        }
        if (!list.Any())
            list = null;

        BO.Task task = new BO.Task
        {
            Id = 0,
            Alias = alias,
            Description = description,
            Status = BO.TaskStatus.Unscheduled,
            Employee = null,
            Dependencies = list,
            CreatedAtDate = DateTime.Now,
            ScheduledDate = null,
            StartDate = null,
            CompleteDate = null,
            ForecastDate = null,
            RequiredEffortTime = requiredEffortTime,
            Deliverables = deliverables,
            Remarks = remarks,
            Complexity = (BO.Type)complexity
        };
        Console.WriteLine(s_bl.Task.Create(task));
    }

    static void ReadT()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWrongValueException("WORNG ID");
        Console.WriteLine(s_bl.Task.Read(id));
    }

    static void ReadAllT()
    {
        List<BO.TaskInList> list;
        list = s_bl.Task.ReadAllTaskInList().ToList();
        foreach (TaskInList task in list)
            Console.WriteLine(task);
    }

    static void UpdateT()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int taskId))
            throw new BlWrongValueException("WORNG ID");
        Console.WriteLine(s_bl.Task.Read(taskId));

        BO.Task task = s_bl.Task.Read(taskId)!;

        string alias = task.Alias??" ";
        string description = task.Description ?? " ";
        BO.Type complexity = task.Complexity;
        TimeSpan? requiredEffortTime = task.RequiredEffortTime;
        DateTime? startDate = task.StartDate;
        DateTime? scheduledDate = task.ScheduledDate;
        DateTime? completeDate = task.CompleteDate;
        string? deliverables = task.Deliverables;
        string? remarks = task.Remarks;
        BO.EmployeeInTask? workOnTask = task.Employee;


        Console.WriteLine("If you want to change the alias of the task enter the new alias, else press no");
        string newAlias = Console.ReadLine()!;
        if (newAlias != "no")
            alias = newAlias;

        Console.WriteLine("If you want to change the description of the task enter the new description, else press no");
        string newDescription = Console.ReadLine()!;
        if (newDescription != "no")
            description = newDescription;

        Console.WriteLine("If you want to change the complexity of the task enter the new complexity, else press -1");
        if (!int.TryParse(Console.ReadLine(), out int newComplexity))
            throw new BlWrongValueException("WORNG LEVEL");
        if (newComplexity != -1)
            complexity = (BO.Type)newComplexity;

        Console.WriteLine("If you want to change the deliverables of the task enter the new deliverables, else press no");
        string newDeliverables = Console.ReadLine()!;
        if (newDeliverables != "no")
            deliverables = newDeliverables;

        Console.WriteLine("If you want to change the remarks of the task enter the new remarks, else press no");
        string newRemarks = Console.ReadLine()!;
        if (newRemarks != "no")
            remarks = newRemarks;

        Console.WriteLine("If you want to change the scheduled date enter yes else enter no");
        string scheduledChoice = Console.ReadLine()!;
        if (scheduledChoice == "yes")
        {
            Console.WriteLine("enter the new scheduled date:");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime newScheduledDate))
                throw new BlWrongValueException("WORNG DATE");
            scheduledDate = newScheduledDate;
        }

        Console.WriteLine("If you want to change the required effort time enter yes else enter no");
        string choiseRequiredEffortTime = Console.ReadLine()!;
        if (choiseRequiredEffortTime == "yes")
        {
            Console.WriteLine("enter the new required effort time:");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan newRequiredEffortTime))
                throw new BlWrongValueException("WORNG DATE");
            requiredEffortTime = newRequiredEffortTime;
        }

        Console.WriteLine("If you want to change the worker who work on this task enter yes else enter no");
        string answer = Console.ReadLine()!;
        if (answer == "yes")
        {
            Console.WriteLine("Enter the ID of the worker:");
            if (!int.TryParse(Console.ReadLine(), out int workerId))
                Console.WriteLine("Enter Name of the worker:");
            string name = Console.ReadLine()!;
            workOnTask = new BO.EmployeeInTask { Id = workerId, Name = name };
        }

        List<BO.TaskInList> dependencies = new List<BO.TaskInList>();
        Console.WriteLine("If you want to change the dependencies of this task enter yes else enter no");
        answer = Console.ReadLine()!;
        if (answer == "yes")
        {
            IEnumerable<BO.TaskInList> tasks = s_bl.Task.ReadAllTaskInList();
            do
            {
                Console.WriteLine($"Enter the ID of the task on which the task depends:");
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out int id))
                        throw new BlWrongValueException("WORNG ID");
                    BO.TaskInList? taskInList = tasks.FirstOrDefault(task => task.Id == id);
                    if (taskInList != null)
                        dependencies.Add(taskInList);
                    else
                        throw new BlDoesNotExistException($"Task with ID={id} doe's NOT exists");
                }
                catch (BlDoesNotExistException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("If you want to add more dependency enter 'yes' otherwise enter 'no':");
                answer = Console.ReadLine()!;
            }
            while (answer == "yes");
        }

        BO.Task taskToUpdate = new BO.Task
        {
            Id = task.Id,
            Alias = alias,
            Description = description,
            Status = task.Status,
            Employee = workOnTask,
            Dependencies = dependencies,
            CreatedAtDate = task.CreatedAtDate,
            ScheduledDate = scheduledDate,
            StartDate = task.StartDate,
            CompleteDate = task.CompleteDate,
            ForecastDate = task.ForecastDate,
            RequiredEffortTime = requiredEffortTime,
            Deliverables = deliverables,
            Remarks = remarks,
            Complexity = complexity
        };

        s_bl.Task.Update(taskToUpdate);
    }

    static void DeleteT()
    {
        Console.WriteLine("Enter ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWrongValueException("WORNG ID");
        s_bl.Task.Delete(id);
    }

    //
    static void ListOfTasksForWorker() 
    {
        Console.WriteLine("Enter  worker Id:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new BlWrongValueException("WORNG ID");
        IEnumerable<BO.TaskInList>? tasksForWorker;
        tasksForWorker = s_bl.Task.TasksForWorker(id);
        if (tasksForWorker != null)
        {
            foreach (TaskInList task in tasksForWorker)
                Console.WriteLine(task);
        }
    }
}
