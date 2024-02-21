using BlApi;
using BO;

namespace BlImplementation;

internal class EmployeeImplementation : BlApi.IEmployee
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// Creates a new employee in the system.
    /// </summary>
    /// <param name="boEmployee">A BO.Employee object containing the employee's details.</param>
    /// <returns>The ID of the created employee, or 0 in case of error.</returns>
    /// <exception cref="BlWrongValueException">If any of the fields have invalid values (negative ID, negative hourly rate, or invalid email).</exception>
    /// <exception cref="BlNullPropertyException">If the employee's name is empty.</exception>
    /// <exception cref="BO.BlAlreadyExistsException">If an employee with the same ID already exists.</exception>
    public int Create(BO.Employee boEmployee)
    {
        // Checks if the object values are valid.
        if (int.IsNegative(boEmployee.Id) || int.IsNegative(boEmployee.HourlyRate) || !Tools.IsValidEmail(boEmployee.Email))
        {
            throw new BlWrongValueException("The employee has WORNG VALUE!");
        }
        // Checks if the employee's name is empty.
        if (string.IsNullOrEmpty(boEmployee.Name))
            throw new BlNullPropertyException("The employee has Null Property!");
        DO.WorkStatus? status = null;
        DO.Type? type = null;
        int? t = (int?)boEmployee.Type;
        int? s = (int?)boEmployee.Status;
        if (t is not null)
        { type = (DO.Type)t; }
        if (s is not null)
        { status = (DO.WorkStatus)s; }
        DO.Employee doEmployee = new(boEmployee.Id, boEmployee.Name, boEmployee.Email, boEmployee.HourlyRate, status, type);
        try
        {
            int idEmp = _dal.Employee.Create(doEmployee);
            return idEmp;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Employee with ID={boEmployee.Id} already exists", ex);
        }
    }
    /// <summary>
    /// Gets an employee's details based on their ID, or null if the employee does not exist.
    /// </summary>
    /// <param name="id">The ID of the employee.</param>
    /// <returns>A BO.Employee object with the employee's details, or null if the employee does not exist.</returns>
    /// <exception cref="BO.BlDoesNotExistException">If the employee with ID id does not exist.</exception>
    public BO.Employee? Read(int id)
    {
        DO.Employee? doEmployee = _dal.Employee.Read(id);

        if (doEmployee == null)// Checks if the employee is not exists.
        {
            throw new BO.BlDoesNotExistException($"Employee with ID={id} does not exist");
        }

        return new BO.Employee()
        {// Converts a DO.Employee object to a BO.Employee object.
            Id = id,
            Name = doEmployee.Name,
            Email = doEmployee.Email,
            HourlyRate = doEmployee.HourlyRate,
            Status = (BO.WorkStatus?)doEmployee.WorkStatus,
            Type = (BO.Type?)doEmployee.Type
        };
    }
    /// <summary>
    /// Gets a list of all employees in the system, or a filtered list based on a filter function.
    /// </summary>
    /// <param name="filter">A filter function to get a filtered list (optional).</param>
    /// <returns>An IEnumerable<BO.EmployeeInTask> list containing the employee details.</returns>

    public IEnumerable<BO.Employee> ReadAll(Func<DO.Employee, bool>? filter = null)
    {
        IEnumerable<DO.Employee?> doEmployees;
        if (filter is not null)
            doEmployees = _dal.Employee.ReadAll(filter);
        else
            doEmployees = _dal.Employee.ReadAll();
        return (from doEmployee in doEmployees
                select new BO.Employee()
                {
                    Id = doEmployee.Id,
                    Name = doEmployee.Name,
                    Email = doEmployee.Email,
                    HourlyRate = doEmployee.HourlyRate,
                    Status = (BO.WorkStatus?)doEmployee.WorkStatus,
                    Type = (BO.Type?)doEmployee.Type,
                    CurrentTaskId = Tools.GetTaskInEmployee(doEmployee.Id)
                });
    }
    //public IEnumerable<BO.Employee> ReadAll(Func<DO.Employee, bool>? filter = null)
    //{
    //    IEnumerable<DO.Employee?> doEmployees;// Reads the list of the all employees from the DAL.
    //    if (filter is not null)
    //        doEmployees = _dal.Employee.ReadAll(filter);
    //    else
    //        doEmployees = _dal.Employee.ReadAll();
    //    return (from doEmployee in doEmployees
    //            select new BO.Employee()
    //            {
    //                Id = doEmployee.Id,
    //                Name = doEmployee.Name,
    //                Email = doEmployee.Email, //we need to complite it
    //            }) ;
    //}
    /// <summary>
    /// Deletes an employee from the system.
    /// </summary>
    /// <param name="idEmp">The ID of the employee.</param>
    /// <exception cref="BO.BlDeletionImpossible">If the employee has tasks that are not in "Unscheduled" or "Scheduled" status.</exception>
    /// <exception cref="BO.BlDoesNotExistException">If the employee with ID idEmp does not exist.</exception>
    public void Delete(int idEmp)
    {
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll(task => task.EmployeeId == idEmp);
        //foreach (var task in tasks)
        //{
        //    TaskStatus taskStatus = Tools.GetStatus(task!);
        //    if (taskStatus != TaskStatus.Unscheduled && taskStatus != TaskStatus.Scheduled)
        //    {
        //        throw new BO.BlDeletionImpossible($"Cannot delete an Employee with task in {taskStatus} status");
        //    }
        //}
        //Tasks that have started executing them cannot be deleted.
        if ( tasks.Select(task => Tools.GetStatus(task!)).Where(taskStatus=> taskStatus != BO.TaskStatus.Unscheduled && taskStatus != BO.TaskStatus.Scheduled).Any())
        {
            throw new BO.BlDeletionImpossible($"Cannot delete an Employee with task in Unscheduled/Scheduled status");
        }
        try
        {
            _dal.Employee.Delete(idEmp);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Worker with ID={idEmp} does NOT exist", ex);
        }
    }
    /// <summary>
    /// Updates an employee's details in the system.
    /// </summary>
    /// <param name="boEmployee">A BO.Employee object containing the updated details.</param>
    /// <exception cref="BlWrongValueException">If the values in the variables are not valid 
    /// (negative ID, negative hourly rate, invalid email, attempt to downgrade employee type).</exception>
    /// <exception cref="BlNullPropertyException">If the employee's name is empty.</exception>
    /// <exception cref="BlNotAppropriateTheProjectStageException">If trying to change the employee's task in a stage that is not ExecutionStage.</exception>
    /// <exception cref="BlDoesNotExistException">If the employee with ID boEmployee.Id does not exist.</exception>
    public void Update(BO.Employee boEmployee)
    {
        // Checks if the object values are valid.
        if (int.IsNegative(boEmployee.Id) || int.IsNegative(boEmployee.HourlyRate) || !Tools.IsValidEmail(boEmployee.Email))
        {
            throw new BlWrongValueException("The employee has WORNG VALUE!");
        }
        if (string.IsNullOrEmpty(boEmployee.Name))
            throw new BlNullPropertyException("The employee has Null Property!");
        var emp = _dal.Employee.Read(emp => emp.Id == boEmployee.Id);// read the employee from DAL
        
        int? tB = null;
        int? tD = null;
        if (emp != null)
        {
            tD = (int?)emp.Type;
            tB = (int?)boEmployee.Type;
        }
        if (emp is not null && tB is not null && tD is not null && tB < tD)//It is not possible to lower the employee's level
        {
            throw new BlWrongValueException("Cannot downgrade employee type");
        }
        //convert STATUS from BO to DO
        DO.WorkStatus? status = null;
        DO.Type? type = null;
        int? t = (int?)boEmployee.Type;
        int? s = (int?)boEmployee.Status;
        if (t is not null)
        { type = (DO.Type)t; }
        if (s is not null)
        { status = (DO.WorkStatus)s; }
        try// Updates the employee's details.
        {
            
            if (boEmployee.CurrentTaskId is not null &&
            Tools.GetCurrentTaskId(boEmployee.Id) != boEmployee.CurrentTaskId.Id) // you realy changed the id of the task
            {
                BO.ProjectStatus? statusProject = IBl.GetProjectStatus();
                if (statusProject != BO.ProjectStatus.ExecutionStage) //updating id of task can only be during the Execution Stage
                    throw new BlNotAppropriateTheProjectStageException("The project is not in the Execution process—you can't update the id of the task");
                if (!Tools.CanTaskBeAssignedFor(boEmployee.CurrentTaskId.Id, boEmployee.Id))
                {
                    throw new BlWrongValueException("Someone is already working on the task you updated");
                }
                if (Tools.IsEmployeeWorkingOnTask(boEmployee.Id))
                {
                    throw new BlWrongValueException("The employee is in the middle of working on a task, you cannot update  a new task for him");
                }

                DO.Task? task;
                DO.Task updatedTask;
                    task = _dal.Task.Read(boEmployee.CurrentTaskId.Id);
                if (task != null)
                {
                    updatedTask = new
                    (
                        task.Id,
                        boEmployee.Id,
                        task.Alias,
                        task.Description,
                        task.CreatedAtDate,
                        task.RequiredEffortTime,
                        task.IsMilestone,
                        task.Complexity,
                        task.StartDate,
                        task.ScheduledDate,
                        task.DeadlineDate,
                        task.CompleteDate,
                        task.Deliverables,
                        task.Remarks
                    );

                    _dal.Task.Update(updatedTask);
                    
                }
            }
                DO.Employee doEmployee = new(boEmployee.Id, boEmployee.Name, boEmployee.Email, boEmployee.HourlyRate, status, type);
                _dal.Employee.Update(doEmployee);
            
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Employee with ID={boEmployee.Id} already exists", ex);
        }
    }
    /// <summary>
    /// Gets a list of all employees in the system, sorted by name.
    /// </summary>
    /// <returns>A List<EmployeeInTask> list containing the employee details, sorted by name.</returns>
    public List<EmployeeInTask>? GetSortedEmployees()
    {
        IEnumerable<DO.Employee?> employees = _dal.Employee.ReadAll();
        // Filters the employees and sorts them by name.
        List<EmployeeInTask>? employeeList=null;
        if (employees != null)
            employeeList = employees.Select(emp => new EmployeeInTask()
           {
            Id = emp!.Id,
            Name = emp.Name
           }).OrderBy(emp => emp.Name).ToList();
        return employeeList;// Returns the sorted list of employees.
    }
}
