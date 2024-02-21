namespace BO;
public enum WorkStatus
{
    Active, Passive, Terminated
}
public enum Type
{
    Beginner, AdvancedBeginner, Intermediate, Advanced, Expert, All
}
public enum TaskStatus
{
    Unscheduled, Scheduled, OnTrack , Done  
}
public enum ProjectStatus
{
    PlanningStage, IntermediateStage, ExecutionStage
}

public enum Entity
{
    Exit,
    Employee,
    Task,
    Schedule
}

public enum MenuEmployee
{
    Exit,
    Add,
    Print,
    PrintListOfEmployees,
    Update,
    Delete,
    StartTask,
    EndTask,
    SignForTask
}
public enum MenuTask
{
    Exit,
    Add,
    Print,
    PrintListOfTasks,
    Update,
    Delete,
    ListOfTasksThatEmployeeCanDo
}


