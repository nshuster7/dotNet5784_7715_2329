using BlApi;
using BO;
using System.Data;
namespace BlImplementation;
internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// Creates a new task in the system.
    /// </summary>
    /// <param name="boTask">The task object to be created.</param>
    /// <returns>The ID of the newly created task.</returns>
    /// <exception cref="BlNotAppropriateTheProjectStageException">
    /// Thrown when attempting to create a task in a project that is not in the planning stage.
    /// </exception>
    /// <exception cref="BlWrongValueException">
    /// Thrown when the task ID is negative, indicating an invalid value.
    /// </exception>
    /// <exception cref="BlNullPropertyException">
    /// Thrown when a required property of the task is null or empty.
    /// </exception>
    /// <exception cref="BO.BlAlreadyExistsException">
    /// Thrown when attempting to create a task that already exists in the system.
    /// </exception>
    public int Create(BO.Task boTask)
    {
        BO.ProjectStatus? status = IBl.GetProjectStatus();
        if (status != BO.ProjectStatus.PlanningStage)//Tasks can only be added during the planning phase
            throw new BlNotAppropriateTheProjectStageException("The project is not in the planning process—you can't add a task");
        else
        {
            if (int.IsNegative(boTask.Id))//Checking if the ID is negative
                throw new BlWrongValueException("The task has WORNG VALUE!");
            if (string.IsNullOrEmpty(boTask.Alias))//Checking if the task has a name
                throw new BlNullPropertyException("The task has Null Property!");
            //Converting the task level from BO to DO
            int? temp = (int?)boTask.Complexity;
            DO.Type? complexity = null;
            if (temp != null)
                complexity = (DO.Type)temp;
            //-------------------------------------
            if (boTask.Dependencies != null)// A task has a list of tasks it depends on, create those dependencies
            {
                //foreach (BO.TaskInList item in boTask.Dependencies)
                //{
                //    DO.Dependency dependency = new DO.Dependency(0, boTask.Id, item.Id);
                //    _dal.Dependency.Create(dependency);
                //}
                boTask.Dependencies.Select(dependency => _dal.Dependency.Create(new DO.Dependency(0, boTask.Id, dependency.Id)));

            }
            DO.Task doTask = new DO.Task
      (
        boTask.Id,
        0,
        boTask.Alias,
        boTask.Description,
        boTask.CreatedAtDate,
        boTask.RequiredEffortTime,
        false,
        complexity,
        boTask.StartDate,
        boTask.ScheduledDate,
        null,
        boTask.CompleteDate,
        boTask.Deliverables,
        boTask.Remarks
      );
            try
            {
                int idTask = _dal.Task.Create(doTask);
                return idTask;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Task? Read(int id)// מחזירה משימה בתצוגה של בו    
    {
        DO.Task? task = _dal.Task.Read(id);//נפעיל את פונקצית הקריאה של שכבת הדאל  

        if (task == null)
        {
            throw new BO.BlDoesNotExistException($"task with ID={id} does not exist");
        }

        return new BO.Task()// נחזיר את המשימה בתוספת תכונות נוספות שלא קיימות במשימה של שכבת הדאל
        {
            Id = id,
            Alias = task.Alias,
            Description = task.Description,
            CreatedAtDate = task.CreatedAtDate,
            Status = Tools.GetStatus(task),
            Dependencies = GetListOfPreviousTask(task.Id),
            RequiredEffortTime = task.RequiredEffortTime,
            StartDate = task.StartDate,
            ForecastDate = Tools.GetMaxDate(task.StartDate, task.ScheduledDate)!.Value.Add(task.RequiredEffortTime ?? TimeSpan.MinValue),
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Employee = GetEmployeeInTask(task.EmployeeId),
            Complexity = GetComplexity(task) ?? 0
        };



    }
    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {

        IEnumerable<DO.Task?> doTask;
        if (filter is not null)
            doTask = _dal.Task.ReadAll(filter);
        else
            doTask = _dal.Task.ReadAll();
        return (from task in doTask
                select new BO.Task()
                {
                    Id = task.Id,
                    Alias = task.Alias,
                    Description = task.Description,
                    CreatedAtDate = task.CreatedAtDate,
                    Status = Tools.GetStatus(task),
                    Dependencies = GetListOfPreviousTask(task.Id),
                    RequiredEffortTime = task.RequiredEffortTime,
                    StartDate = task.StartDate,
                    ForecastDate = Tools.GetMaxDate(task.StartDate, task.ScheduledDate)!.Value.Add(task.RequiredEffortTime ?? TimeSpan.MinValue),
                    CompleteDate = task.CompleteDate,
                    Deliverables = task.Deliverables,
                    Remarks = task.Remarks,
                    Employee = GetEmployeeInTask(task.EmployeeId),
                    Complexity = GetComplexity(task) ?? 0
                });
    }
    public IEnumerable<BO.TaskInList> ReadAllTaskInList(Func<DO.Task, bool>? filter = null)
    {
        IEnumerable<DO.Task?> doTask;
        if (filter is not null)
            doTask = _dal.Task.ReadAll(filter);
        else
            doTask = _dal.Task.ReadAll();
        return (from task in doTask
                select new BO.TaskInList()
                {
                    Id = task.Id,
                    Alias = task.Alias,
                    Description = task.Description,
                    Status = Tools.GetStatus(task)
                });
    }
    public void Delete(int idTask)
    {
        BO.ProjectStatus? status = IBl.GetProjectStatus();
        if (status != BO.ProjectStatus.PlanningStage)//Tasks can only be added during the planning phase
            throw new BlNotAppropriateTheProjectStageException("The project is not in the planning process—you can't delete a task");
        DO.Task? task = _dal.Task.Read(idTask);
        if (task == null)
        {
            throw new BlDoesNotExistException($"task with ID={idTask} does not exist");
        }
        if (!CanTaskBeDeleted(task))
        {
            throw new BlDeletionImpossible("Task cannot be deleted because it is either assigned to an employee or has incomplete dependent tasks.");
        }
        try
        {
            _dal.Task.Delete(idTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDeletionImpossible($"Failed to delete task from data layer: {ex.Message}", ex);
        }
    }
    public void Update(BO.Task task)
    {
        BO.ProjectStatus projectStatus = IBl.GetProjectStatus();
        if ( projectStatus == BO.ProjectStatus.ExecutionStage)
        {
            try
            {
                DO.Task? checkingTask = _dal.Task.Read(task.Id);
                // In IntermediateStage or ExecutionStage you cant update the times or dates
                if (checkingTask != null && (task.CreatedAtDate != checkingTask.CreatedAtDate || (int?)task.Complexity != (int?)checkingTask.Complexity ||
                    task.RequiredEffortTime != checkingTask.RequiredEffortTime || task.StartDate != checkingTask.StartDate || task.ScheduledDate != checkingTask.StartDate 
                    || task.CompleteDate != checkingTask.CompleteDate))
                    throw new BlNotAppropriateTheProjectStageException("You cannot update the task in this stage at the project");
                    if (checkingTask != null)
                    {
                    int workerId = 0;
                    if (task.Employee != null)
                        workerId = task.Employee.Id;
                    
                    checkingTask = checkingTask with { Alias = task.Alias, Description = task.Description, Deliverables = task.Deliverables, Remarks = task.Remarks, EmployeeId = workerId };
                    _dal.Task.Update(checkingTask);
                    }
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Task with ID={task.Id} doe's NOT exists", ex);
            }
        }

        else
        {
            if (task.Dependencies != null)
            {
                DO.Task? unupdatedT = _dal.Task.Read(task.Id);
                //IEnumerable<Dependency> t = from TaskInList item in task.Dependencies
                //        where _dal.Dependency.Check(task.Id, item.Id) == null
                //        select new DO.Dependency(0, task.Id, item.Id);
                //t.ToList().ForEach(dependency =>
                //{
                //    _dal.Dependency.Create(dependency);
                //});
                task.Dependencies.Where(t => _dal.Dependency.Check(task.Id, t.Id) == null).Select(dependency => _dal.Dependency.Create(new DO.Dependency(0, task.Id, dependency.Id)));
            }

            DO.Task doTask = DataChecking(task);

            try
            {
                _dal.Task.Update(doTask);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Task with ID={task.Id} doe's NOT exists", ex);
            }
        }
    }
    private DO.Task DataChecking(BO.Task task)
    {
        int idEmp = 0;
        if (int.IsNegative(task.Id))//Checking if the ID is negative
            throw new BlWrongValueException("The task has WORNG VALUE!");
        if (string.IsNullOrEmpty(task.Alias))//Checking if the task has a name
            throw new BlNullPropertyException("The task has Null Property!");
        int? temp = (int?)task.Complexity;
        DO.Type? complexity = null;
        if (temp != null)
            complexity = (DO.Type)temp;

        if (task.ScheduledDate != null)
        {
            BO.ProjectStatus projectStatus = IBl.GetProjectStatus();
            if (projectStatus == BO.ProjectStatus.PlanningStage)
                throw new BlNotAppropriateTheProjectStageException("You cannot enter a scheduled start date for a task at this stage of the project");
            else
                UpdateScheduledStartDate(task.Id, (DateTime)task.ScheduledDate);
        }

        int? workerId = null;
        if (task.Employee != null)
            workerId = task.Employee.Id;
        if(task.Employee!=null)
            idEmp = task.Employee.Id;

        DO.Task doTask = new DO.Task(task.Id, idEmp, task.Alias, task.Description, task.CreatedAtDate, task.RequiredEffortTime, false, complexity, task.StartDate, task.ScheduledDate, null, task.CompleteDate, task.Deliverables, task.Remarks);

        return doTask;
    }
    public void UpdateScheduledStartDate(int taskId, DateTime plannedStartDate)
    {
        // 1. Validate task ID:
        if (taskId <= 0)
        {
            throw new BlWrongValueException("Invalid task ID. Must be a positive integer.");
        }

        // 2. Retrieve task details:
        DO.Task? task = _dal.Task.Read(taskId);
        if (task == null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID {taskId} does not exist.");
        }

        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == taskId);// רשימת כל התלויות שהמשימה הנוכחית שלי תלויה תלוי במשימה אחרת
        
        foreach (DO.Dependency? dependency in dependencies)
        {
            DO.Task? precedingTask;
            if (dependency != null)
            {
                precedingTask = _dal.Task.Read(dependency.DependsOnTask ?? 0);
                if (precedingTask == null || precedingTask.ScheduledDate == null)
                {
                    throw new BlDataException($"Cannot update scheduled start date: " +
                                              $"Preceding task {dependency.DependsOnTask} has no forecast start date.");
                }
            }
        }

        // 4. Check for early start date compared to preceding task forecast dates:
        foreach (DO.Dependency? dependency in dependencies)
        {
            if (dependency != null)
            {
                DO.Task? precedingTask = _dal.Task.Read(dependency.DependsOnTask ?? 0);
                DateTime? forecast = Tools.GetMaxDate(precedingTask!.StartDate, precedingTask.ScheduledDate)!.Value.Add(precedingTask.RequiredEffortTime ?? TimeSpan.MinValue);
                if (forecast != null && plannedStartDate < forecast)
                {
                    throw new BlWrongValueException($"Planned start date cannot be earlier than forecast date " +
                                              $"of any preceding task: {precedingTask.Id} forecasted on {forecast.Value}.");
                }
            }
        }

            // 5. Create a new DO.Task object with updated ScheduledDate:
    DO.Task updatedTask = new DO.Task(
    task.Id,
    task.EmployeeId,
    task.Alias,
    task.Description,
    task.CreatedAtDate,
    task.RequiredEffortTime,
    false,
    task.Complexity,
    plannedStartDate,
    task.ScheduledDate,
    null,
    task.CompleteDate,
    task.Deliverables,
    task.Remarks
    );
        // 6. Update the task in the data layer:
        _dal.Task.Update( updatedTask);
    }
    public bool CanTaskBeDeleted( DO.Task task)
    {
        DO.Task? task1 = _dal.Task.Read(task.Id);
        if (task1 == null) return false;
        var dep = _dal.Dependency.ReadAll(d => d.DependsOnTask == task.Id);
        if (dep != null) return false;
        return true;
    }
    //The function receives an ID number of a task and checks whether the task is available
    //i.e. whether there is no one else working on the task and also whether all the tasks preceding it have been performed
    public bool IsTaskAvailable( DO.Task task)
    {
        if (task.EmployeeId != 0)
            return false;
        var dependencies = _dal.Dependency.ReadAll().Where(d => d!.DependentTask == task.Id).ToList();// A list of dependencies when my task depends on others
        List<TaskInList> taskList = (from DO.Dependency d in dependencies
                                     let temporary = _dal.Task.Read(d.DependsOnTask ?? 0) //A list of tasks that the current task depends on
                                     select new TaskInList
                                     {
                                         Id = d.DependsOnTask ?? 0,
                                         Description = temporary.Description,
                                         Alias = temporary.Alias,
                                         Status = Tools.GetStatus(temporary)
                                     }).ToList();
        var result = taskList.Where(prevTask => prevTask.Status != BO.TaskStatus.Done);
        if(result.Any())
           return false;
        //foreach (var prevTask in taskList)
        //{
        //    if (prevTask.Status != BO.TaskStatus.Done)
        //        return false;
        //}
        return true;
    }
    public List<TaskInList> GetListOfPreviousTask(int id)
    {
        List<TaskInList> taskList = (from DO.Dependency d in _dal.Dependency.ReadAll()
                                     where d.DependentTask == id
                                     let temporary = _dal.Task.Read(d.DependsOnTask ?? 0)
                                     select new TaskInList
                                     {
                                         Id = d.DependsOnTask ?? 0,
                                         Description = temporary.Description,
                                         Alias = temporary.Alias,
                                         Status = Tools.GetStatus(temporary)
                                     }).ToList();
        return taskList;
    }
    public  BO.Type? GetComplexity(DO.Task task)
    {
        int? temp = (int?)task.Complexity;
        BO.Type? complexity = null;
        if (temp != null)
            return complexity = (BO.Type)temp;
        return null;
    }
    public IEnumerable<BO.TaskInList>? TasksForWorker(int empId)
    {
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();
        return from DO.Task? item in tasks
               let task = _dal.Task.Read(item.Id)
               where Tools.CanTaskBeAssignedFor(item.Id, empId) && IsTaskAvailable(task)
               select new BO.TaskInList { Id = item.Id, Alias = item.Alias, Description = item.Description, Status = Tools.GetStatus(item) };
    }
    public  string? GetCurrentTaskAlias( int? idEmp)
    {
        DO.Task? t = _dal.Task.Read(task => task.EmployeeId == idEmp);
        if (t is not null)
            return t.Alias;
        return null;
    }
    public  TaskInEmployee? GetTaskInEmployee( int? idEmp)
    {
        int? id = Tools.GetCurrentTaskId(idEmp);
        if (id == null) return null;
        return new BO.TaskInEmployee { Id = (int)id, Alias = GetCurrentTaskAlias(idEmp) };
    }
    public  EmployeeInTask? GetEmployeeInTask(int idEmp)
    {
        DO.Employee? emp = _dal.Employee.Read(idEmp);
        if (emp is not null)
            return new EmployeeInTask { Id = idEmp, Name = emp.Name };
        return null;
    }
    public void StartTask(int idT, int idEmp)
    {
        DO.Task? task = _dal.Task.Read(idT);
        if (task == null)
        {
            throw new BlDoesNotExistException($"Task with ID {idT} does not exist.");
        }
        if(task.EmployeeId != idEmp)
            throw new BlDataException($"Task with ID {idT} has done by someone else.");

        if (task.StartDate.HasValue && task.StartDate!= DateTime.Now)
        {
            throw new BlDataException($"Task with ID {idT} has already started.");
        }
        var newT=task with { CreatedAtDate = DateTime.Now };

        _dal.Task.Update(newT);
    }
    public void EndTask(int idT, int idEmp)
    {
        DO.Task? task = _dal.Task.Read(idT);
        if (task == null)
        {
            throw new BlDoesNotExistException($"Task with ID {idT} does not exist.");
        }

        if (task.CompleteDate.HasValue)
        {
            throw new BlDataException($"Task with ID {idT} has already been completed.");
        }
        var newT = task with { CompleteDate = DateTime.Now };
        _dal.Task.Update(newT);
    }
    public void SignUpForTask(int idT, int idEmp)
    {

        // 1. נבדוק אם העובד והמשימה קיימים
        DO.Task? task = _dal.Task.Read(idT);
        if (task == null)
        {
            throw new BlDoesNotExistException($"Task with ID {idT} does not exist.");
        }

        DO.Employee? employee = _dal.Employee.Read(idEmp);
        if (employee == null)
        {
            throw new BlDoesNotExistException($"Employee with ID {idEmp} does not exist.");
        }

        if (!IsTaskAvailable(task))
        {
            throw new BlTaskCantBeAssignedException($"Task with ID {idT} is not available for assignment.");
        }

        if (!Tools.IsEmployeeWorkingOnTask(idEmp))
        {
            throw new Exception($"Employee with ID {idEmp} is already assigned to another task.");
        }
        var newT = task with { EmployeeId = idEmp };
        _dal.Task.Update(newT);
    }
 
}
