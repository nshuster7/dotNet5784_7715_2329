using BlApi;
using BO;
using DalApi;
using DO;
using System.Data;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
            //
            if (boTask.Dependencies != null)
            {
                A task has a list of tasks it depends on, create those dependencies
                foreach (BO.TaskInList item in boTask.Dependencies)
                {
                    DO.Dependency dependency = new DO.Dependency(0, boTask.Id, item.Id);
                    _dal.Dependency.Create(dependency);
                }

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
    public BO.Task? Read(int id)
    {
            DO.Task? task = _dal.Task.Read(id);

            if (task == null)
            {
                throw new BO.BlDoesNotExistException($"task with ID={id} does not exist");
            }
          
             return new BO.Task()
            {
                Id = id,
                Alias = task.Alias,
                Description = task.Description,
                CreatedAtDate = task.CreatedAtDate,
                Status = Tools.GetStatus(task),
                Dependencies = Tools.GetListOfPreviousTask(_dal,task.Id),
                RequiredEffortTime = task.RequiredEffortTime,
                StartDate = task.StartDate,
                ForecastDate = Tools.GetMaxDate(task.StartDate, task.ScheduledDate)!.Value.Add(task.RequiredEffortTime ?? TimeSpan.MinValue),
                CompleteDate= task.CompleteDate,
                Deliverables=task.Deliverables,
                Remarks=task.Remarks,
                Employee=Tools.GetEmployeeInTask(_dal,task.EmployeeId),
                Complexity=Tools.GetComplexity(_dal,task)??0
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
            Dependencies = Tools.GetListOfPreviousTask(_dal, task.Id),
            RequiredEffortTime = task.RequiredEffortTime,
            StartDate = task.StartDate,
            ForecastDate = Tools.GetMaxDate(task.StartDate, task.ScheduledDate)!.Value.Add(task.RequiredEffortTime ?? TimeSpan.MinValue),
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Employee = Tools.GetEmployeeInTask(_dal, task.EmployeeId),
            Complexity = Tools.GetComplexity(_dal, task) ?? 0
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
        if (!Tools.CanTaskBeDeleted(_dal, task))
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
                IEnumerable<Dependency> t = from TaskInList item in task.Dependencies
                        where _dal.Dependency.Check(task.Id, item.Id) == null
                        select new DO.Dependency(0, task.Id, item.Id);
                t.ToList().ForEach(dependency =>
                {
                    _dal.Dependency.Create(dependency);
                });
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

    public void Clear()
    {
        _dal.Task.Clear();
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

        // 3. Ensure all preceding tasks have forecast start dates:
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == taskId);
        
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




}
