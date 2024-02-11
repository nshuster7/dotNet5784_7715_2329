namespace BlImplementation;
using System.Collections.Generic;
using BlApi;
using BO;

internal class EmployeeImplementation : BlApi.IEmployee
{

    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Employee boEmployee)
    {
        if (int.IsNegative(boEmployee.Id) || int.IsNegative(boEmployee.HourlyRate) || !Tools.IsValidEmail(boEmployee.Email))
        {
            throw new BlWrongValueException("The employee has WORNG VALUE!");
        }
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
    public BO.Employee? Read(int id)
    {
        DO.Employee? doEmployee = _dal.Employee.Read(id);

        if (doEmployee == null)
        {
            throw new BO.BlDoesNotExistException($"Employee with ID={id} does not exist");
        }

        return new BO.Employee()
        {
            Id = id,
            Name = doEmployee.Name,
            Email = doEmployee.Email,
            HourlyRate = doEmployee.HourlyRate,
            Status = (BO.WorkStatus?)doEmployee.WorkStatus,
            Type = (BO.Type?)doEmployee.Type
        };
    }

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
                    CurrentTaskId = Tools.GetTaskInEmployee(_dal, doEmployee.Id)
                });
    }
    public void Delete(int idEmp)
    {

        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll(task => task.EmployeeId == idEmp);
        foreach (var task in tasks)
        {
            TaskStatus taskStatus = Tools.GetStatus(task!);
            if (taskStatus != TaskStatus.Unscheduled && taskStatus != TaskStatus.Scheduled)
            {
                throw new BO.BlDeletionImpossible($"Cannot delete an Employee with task in {taskStatus} status");
            }
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
    public void Update(BO.Employee boEmployee)
    {
        if (int.IsNegative(boEmployee.Id) || int.IsNegative(boEmployee.HourlyRate) || !Tools.IsValidEmail(boEmployee.Email))
        {
            throw new BlWrongValueException("The employee has WORNG VALUE!");
        }
        if (string.IsNullOrEmpty(boEmployee.Name))
            throw new BlNullPropertyException("The employee has Null Property!");
        var emp = _dal.Employee.Read(emp => emp.Id == boEmployee.Id);
        int? tB = null;
        int? tD = null;
        if (emp != null)
        {
            tD = (int?)emp.Type;
            tB = (int?)boEmployee.Type;
        }
        if (emp is not null && tB is not null && tD is not null && tB < tD)
        {
            throw new BlWrongValueException("Cannot downgrade employee type");
        }
        DO.WorkStatus? status = null;
        DO.Type? type = null;
        int? t = (int?)boEmployee.Type;
        int? s = (int?)boEmployee.Status;
        if (t is not null)
        { type = (DO.Type)t; }
        if (s is not null)
        { status = (DO.WorkStatus)s; }
        try
        {
            
            if (boEmployee.CurrentTaskId is not null &&
            Tools.GetCurrentTaskId(_dal, boEmployee.Id) != boEmployee.CurrentTaskId.Id) // you realy changed the id of the task
            {
                BO.ProjectStatus? statusProject = IBl.GetProjectStatus();
                if (statusProject != BO.ProjectStatus.ExecutionStage) //updating id of task can only be during the Execution Stage
                    throw new BlNotAppropriateTheProjectStageException("The project is not in the Execution process—you can't update the id of the task");
                if (!Tools.CanTaskBeAssignedFor(_dal, boEmployee.CurrentTaskId.Id, boEmployee.Id))
                {
                    throw new BlWrongValueException("Someone is already working on the task you updated");
                }
                if (Tools.IsEmployeeWorkingOnTask(_dal, boEmployee.Id))
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
    public void Clear()
    {
        _dal.Employee.Clear();
    }
}
