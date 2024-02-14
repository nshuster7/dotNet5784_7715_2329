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
    ///
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
    /// Returns a task in Bo's view
    /// </summary>
    /// <param name="id"></param>
    /// <returns>We'll return the taskin the Dahl layer mission</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// Throws an exception if what we were looking for was not found
    public BO.Task? Read(int id)
    {//Returns a task in Bo's view
        DO.Task? task = _dal.Task.Read(id);//call the reading function of the dal layer  

        if (task == null)
        {
            throw new BO.BlDoesNotExistException($"task with ID={id} does not exist");
        }

        return new BO.Task() //We'll return the task with additional features not present in the Dahl layer mission
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
    /// <summary>
    /// Reads all tasks from the system.
    /// </summary>
    /// <param name="filter">An optional filter function to apply to the list of tasks. 
    /// (Returns true for tasks that should remain in the list and false for those that should be excluded).</param>
    /// <returns>A list of BO.Task objects representing the found tasks.</returns>
    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        IEnumerable<DO.Task?> doTask;
        if (filter is not null) //Check if a filter exist, and if so use it to read tasks from the DAL.
            doTask = _dal.Task.ReadAll(filter);
        else //If a filter doesn't exist, read all tasks from the DAL without a filter.
            doTask = _dal.Task.ReadAll();
        // Convert the list of DO tasks to a list of BO tasks.
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
    /// <summary>
    /// Reads all tasks in a list format from the system.
    /// </summary>
    /// <param name="filter">An optional filter function to apply to the list of tasks. 
    /// (Returns true for tasks that should remain in the list and false for those that should be excluded).</param>
    /// <returns>A list of BO.TaskInList objects representing the found tasks in a concise format.</returns>
    public IEnumerable<BO.TaskInList> ReadAllTaskInList(Func<DO.Task, bool>? filter = null)
    {
        IEnumerable<DO.Task?> doTask;
        if (filter is not null) // Check if a filter exist, and if so use it to read tasks from the DAL.
            doTask = _dal.Task.ReadAll(filter);
        else  // Otherwise, read all tasks from the DAL without a filter.
            doTask = _dal.Task.ReadAll();
        // Convert the list of DO tasks to a list of BO tasks in the list format.
        return (from task in doTask
                select new BO.TaskInList()
                {
                    Id = task.Id,
                    Alias = task.Alias,
                    Description = task.Description,
                    Status = Tools.GetStatus(task)
                });
    }
    /// <summary>
    /// Deletes a task from the system.
    /// </summary>
    /// <param name="idTask">The ID of the task to be deleted.</param>
    /// <exception cref="BlNotAppropriateTheProjectStageException">
    /// Thrown when attempting to delete a task in a project that is not in the planning stage.
    /// </exception>
    /// <exception cref="BlDoesNotExistException">
    /// Thrown when the task with the provided ID does not exist in the system.
    /// </exception>
    /// <exception cref="BlDeletionImpossible">
    /// Thrown when the task cannot be deleted because it is either assigned to an employee or has incomplete dependent tasks.
    /// </exception>
    /// <exception cref="BO.BlDeletionImpossible">
    /// Thrown when the deletion fails at the data layer (DO) level.
    /// </exception>
    public void Delete(int idTask)
    {
        BO.ProjectStatus? status = IBl.GetProjectStatus();
        if (status != BO.ProjectStatus.PlanningStage)  //Tasks can only be added during the planning phase
            // Throws an exception if the project is not in the planning stage.
            throw new BlNotAppropriateTheProjectStageException("The project is not in the planning process—you can't delete a task");
        DO.Task? task = _dal.Task.Read(idTask);
        if (task == null)  //Checks if the task exists.
        {
            throw new BlDoesNotExistException($"task with ID={idTask} does not exist");
        }
        if (!CanTaskBeDeleted(task))  //Checks if the task can be deleted.
        {
            throw new BlDeletionImpossible("Task cannot be deleted because it is either assigned to an employee or has incomplete dependent tasks.");
        }
        try
        {
            _dal.Task.Delete(idTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {  // Throws an exception if the deletion fails.
            throw new BO.BlDeletionImpossible($"Failed to delete task from data layer: {ex.Message}", ex);
        }
    }
    /// <summary>
    /// Updates a task in the system.
    /// </summary>
    /// <param name="task">The updated task object.</param>
    /// <exception cref="BlNotAppropriateTheProjectStageException">
    /// Thrown when attempting to update a task that is not in the planning or execution stage.
    /// </exception>
    /// <exception cref="BlDoesNotExistException">
    /// Thrown when the updated task does not exist in the system.
    /// </exception>
    public void Update(BO.Task task)
    {
        BO.ProjectStatus projectStatus = IBl.GetProjectStatus();  // Gets the current project status.
        if ( projectStatus == BO.ProjectStatus.ExecutionStage)  // Checks if the project is in the execution stage.
        {
            try
            {
                DO.Task? checkingTask = _dal.Task.Read(task.Id);
                // Checks if time and date fields can be updated in the intermediate or execution stages.
                // In ExecutionStage you cant update the times or dates
                if (checkingTask != null && (task.CreatedAtDate != checkingTask.CreatedAtDate || (int?)task.Complexity != (int?)checkingTask.Complexity ||
                    task.RequiredEffortTime != checkingTask.RequiredEffortTime || task.StartDate != checkingTask.StartDate || task.ScheduledDate != checkingTask.StartDate 
                    || task.CompleteDate != checkingTask.CompleteDate))
                    throw new BlNotAppropriateTheProjectStageException("You cannot update the task in this stage at the project");
                    if (checkingTask != null)  // Checks if the task exists.
                {
                    int workerId = 0;
                    if (task.Employee != null)  // Gets the employee ID from the updated task.
                        workerId = task.Employee.Id;
                    // Updates the task in the DAL.
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
            if (task.Dependencies != null)  // Checks if the updated task has dependencies.
            {
                DO.Task? unupdatedT = _dal.Task.Read(task.Id);
                //IEnumerable<Dependency> t = from TaskInList item in task.Dependencies
                //        where _dal.Dependency.Check(task.Id, item.Id) == null
                //        select new DO.Dependency(0, task.Id, item.Id);
                //t.ToList().ForEach(dependency =>
                //{
                //    _dal.Dependency.Create(dependency);
                //});
                // Creates a new list of dependencies for the task.
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
    /// <summary>
    /// Performs data checks on a task before updating the task in the DAL.
    /// </summary>
    /// <param name="task">The updated task object.</param>
    /// <returns>The task object after performing data checks.</returns>
    /// <exception cref="BlWrongValueException">
    /// Thrown when the task ID is negative.
    /// </exception>
    /// <exception cref="BlNullPropertyException">
    /// Thrown when the task name is empty or null.
    /// </exception>
    /// <exception cref="BlNotAppropriateTheProjectStageException">
    /// Thrown when attempting to enter a scheduled start date in the planning stage of the project.
    /// </exception>
    private DO.Task DataChecking(BO.Task task)
    {
        int idEmp = 0;
        if (int.IsNegative(task.Id))//Checking if the ID is negative
            throw new BlWrongValueException("The task has WORNG VALUE!");
        if (string.IsNullOrEmpty(task.Alias))//Checking if the task has a name
            throw new BlNullPropertyException("The task has Null Property!");// Throws an exception if the task name is empty or null.
        int? temp = (int?)task.Complexity;
        DO.Type? complexity = null;
        if (temp != null)// Updates the variable if the complexity level exists.
            complexity = (DO.Type)temp;

        if (task.ScheduledDate != null)// Checks if a scheduled start date has been entered.
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
    /// <summary>
    /// Updates the scheduled start date of a task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="plannedStartDate">The new planned start date.</param>
    /// <exception cref="BlWrongValueException">
    /// Thrown when the task ID is invalid or when the planned start date is too early relative to the forecast dates of dependent tasks.
    /// </exception>
    /// <exception cref="BO.BlDoesNotExistException">
    /// Thrown when the task with the provided ID does not exist in the system.
    /// </exception>
    /// <exception cref="BlDataException">
    /// Thrown when the scheduled start date cannot be updated because the task depends on another task that has no forecast start date.
    /// </exception>
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
    /// <summary>
    /// Checks whether a task can be deleted.
    /// </summary>
    /// <param name="task">The task object to be deleted.</param>
    /// <returns>True if the task can be deleted, False otherwise.</returns>
    public bool CanTaskBeDeleted( DO.Task task)
    {
        DO.Task? task1 = _dal.Task.Read(task.Id);
        // If there are tasks dependent on the current task as a preceding task, returns False.
        if (task1 == null) return false;
        var dep = _dal.Dependency.ReadAll(d => d.DependsOnTask == task.Id);
        if (dep != null) return false;
        return true;
    }
    /// <summary>
    ///The function receives an ID number of a task and checks whether the task is available
    ///i.e. whether there is no one else working on the task and also whether all the tasks preceding it have been performed    /// </summary>
    /// Checks whether a task is available for execution.
    /// </summary>
    /// <param name="task">The task object.</param>
    /// <returns>True if the task is available for execution, False otherwise.</returns>
    public bool IsTaskAvailable(DO.Task task)
    {    //The function receives an ID number of a task and checks whether the task is available
         //i.e. whether there is no one else working on the task and also whether all the tasks preceding it have been performed
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
        if (result.Any())
            return false;// Returns False if there is at least one preceding task that is not yet completed.
        //foreach (var prevTask in taskList)
        //{
        //    if (prevTask.Status != BO.TaskStatus.Done)
        //        return false;
        //}
        return true;
    }
    /// <summary>
    /// Gets a list of all preceding tasks for a specific task.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <returns>A list of TaskInList objects representing the preceding tasks.</returns>
    public List<TaskInList> GetListOfPreviousTask(int id)
    {// Creates a list of dependencies where the current task is the dependent task.
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
    /// <summary>
    /// Gets the complexity level of a task.
    /// </summary>
    /// <param name="task">The task object.</param>
    /// <returns>The task's complexity level as a BO.Type, or null if the complexity level is not set.</returns>
    public BO.Type? GetComplexity(DO.Task task)
    {
        int? temp = (int?)task.Complexity;
        BO.Type? complexity = null;
        if (temp != null)// If the complexity level exists, updates the result variable. else return NULL
            return complexity = (BO.Type)temp;
        return null;
    }
    /// <summary>
    /// Gets a list of available tasks that a worker can perform.
    /// </summary>
    /// <param name="empId">The ID of the worker.</param>
    /// <returns>A list of BO.TaskInList objects representing the available tasks, or null if there are no available tasks.</returns>
    public IEnumerable<BO.TaskInList>? TasksForWorker(int empId)
    {
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();
        // Filters the tasks based on tasks that can be assigned to the worker and tasks that are available for execution.
        return from DO.Task? item in tasks
               let task = _dal.Task.Read(item.Id)
               where Tools.CanTaskBeAssignedFor(item.Id, empId) && IsTaskAvailable(task)
               select new BO.TaskInList { Id = item.Id, Alias = item.Alias, Description = item.Description, Status = Tools.GetStatus(item) };
    }
    /// <summary>
    /// Gets the alias of the current task assigned to a specific employee,
    /// or null if the employee does not have a current task.
    /// </summary>
    /// <param name="idEmp">The ID of the employee.</param>
    /// <returns>The alias of the employee's current task, or null.</returns>
    public string? GetCurrentTaskAlias( int? idEmp)
    {
        DO.Task? t = _dal.Task.Read(task => task.EmployeeId == idEmp);
        if (t is not null)
            return t.Alias;
        return null;
    }
    /// <summary>
    /// Gets information about the current task assigned to a specific employee, or null if the employee does not exist or does not have a current task.
    /// </summary>
    /// <param name="idEmp">The ID of the employee.</param>
    /// <returns>A BO.TaskInEmployee object with details of the employee's current task, or null.</returns>
    public TaskInEmployee? GetTaskInEmployee( int? idEmp)
    {
        int? id = Tools.GetCurrentTaskId(idEmp);
        if (id == null) return null;
        // Creates a BO.TaskInEmployee object with the task details.
        return new BO.TaskInEmployee { Id = (int)id, Alias = GetCurrentTaskAlias(idEmp) };
    }
    /// <summary>
    /// Gets information about an employee given their ID, or null if no employee with that ID exists.
    /// </summary>
    /// <param name="idEmp">The ID of the employee.</param>
    /// <returns>An EmployeeInTask object with the employee's details, or null if the employee is not found.</returns>
    public EmployeeInTask? GetEmployeeInTask(int idEmp)
    {
        DO.Employee? emp = _dal.Employee.Read(idEmp);
        if (emp is not null)// If an employee is found, creates an EmployeeInTask object with their details.
            return new EmployeeInTask { Id = idEmp, Name = emp.Name };
        return null;
    }
    /// <summary>
    /// Starts the execution of a task.
    /// </summary>
    /// <param name="idT">The ID of the task.</param>
    /// <param name="idEmp">The ID of the employee who is starting the task.</param>
    /// <exception cref="BlDoesNotExistException">If the task with ID idT does not exist.</exception>
    /// <exception cref="BlDataException">If the task is already assigned to another employee or has already started.</exception>
    public void StartTask(int idT, int idEmp)
    {
        DO.Task? task = _dal.Task.Read(idT);
        if (task == null)// Checks if the task exists.
        {
            throw new BlDoesNotExistException($"Task with ID {idT} does not exist.");
        }
        if(task.EmployeeId != idEmp)// Checks if the task is already assigned to another employee.
            throw new BlDataException($"Task with ID {idT} has done by someone else.");

        if (task.StartDate.HasValue && task.StartDate!= DateTime.Now)// Checks if the task has already started.
        {
            throw new BlDataException($"Task with ID {idT} has already started.");
        }
        var newT=task with { CreatedAtDate = DateTime.Now };

        _dal.Task.Update(newT);
    }
    /// <summary>
    /// Ends the execution of a task.
    /// </summary>
    /// <param name="idT">The ID of the task.</param>
    /// <param name="idEmp">The ID of the employee who is ending the task.</param>
    /// <exception cref="BlDoesNotExistException">If the task with ID idT does not exist.</exception>
    /// <exception cref="BlDataException">If the task has already been completed.</exception>
    public void EndTask(int idT, int idEmp)
    {
        DO.Task? task = _dal.Task.Read(idT);
        if (task == null)// Checks if the task exists.
        {
            throw new BlDoesNotExistException($"Task with ID {idT} does not exist.");
        }
        if (task.CompleteDate.HasValue)// Checks if the task has already been completed.
        {
            throw new BlDataException($"Task with ID {idT} has already been completed.");
        }
        var newT = task with { CompleteDate = DateTime.Now };// Updates the completion date and time of the task.
        _dal.Task.Update(newT);
    }
    /// <summary>
    /// Assigns a task to an employee.
    /// </summary>
    /// <param name="idT">The ID of the task.</param>
    /// <param name="idEmp">The ID of the employee.</param>
    /// <exception cref="BlDoesNotExistException">If the task or employee does not exist.</exception>
    /// <exception cref="BlTaskCantBeAssignedException">If the task is not available for assignment.</exception>
    /// <exception cref="Exception">If the employee is already assigned to another task.</exception>
    public void SignUpForTask(int idT, int idEmp)
    {
        //1. Checks if the employee and task exist
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
        //2. Checks if the task is available for assignment.
        if (!IsTaskAvailable(task))
        {
            throw new BlTaskCantBeAssignedException($"Task with ID {idT} is not available for assignment.");
        }
        //3. Checks if the employee is already assigned to another task
        if (!Tools.IsEmployeeWorkingOnTask(idEmp))
        {
            throw new Exception($"Employee with ID {idEmp} is already assigned to another task.");
        }
        //4. Assigns the task to the employee and updates the DAL.
        var newT = task with { EmployeeId = idEmp };
        _dal.Task.Update(newT);
    }
 
}
