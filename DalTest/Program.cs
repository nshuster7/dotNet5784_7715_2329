//Efrat Aharoni & Noa Shuster
//We used the TryParse
namespace DalTest;
using Dal;
using DalApi;
using DO;
using System;
using System.Reflection.Emit;
using Type = DO.Type;

public enum CRUD
{
    Exit,
    Create,
    Read,
    ReadAll,
    Update,
    Delete,
    ReadDependencyOfTwoTasks
}
public enum Entity
{
    Exit,
    Employee,
    Task,
    Dependency
}
internal class Program
{
    //private static IEmployee? s_dalEmployee = new EmployeeImplementation(); //stage 1
    //private static ITask? s_dalTask = new TaskImplementation(); //stage 1
    //private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
    static readonly IDal s_dal = new DalList();
    private static readonly Random s_rand = new();
    static void Main(string[] args)
    {
        try
        {
            //Initialization.Do(s_dalEmployee, s_dalTask, s_dalDependency);
            Initialization.Do(s_dal); //stage 2
            Entity myChoice;
            do
            {
                Console.WriteLine("choose 0 - exit main menu");
                Console.WriteLine("choose 1 - Employee");
                Console.WriteLine("choose 2 - Task");
                Console.WriteLine("choose 3 - Dependency");
                if (!Enum.TryParse(Console.ReadLine(), out myChoice))
                    throw new Exception("WORNG VALUE");
                switch (myChoice)
                {
                    case Entity.Exit:
                        break;
                    case Entity.Employee:
                        employeeFunc();
                        break;
                    case Entity.Task:
                        taskFunc();
                        break;
                    case Entity.Dependency:
                        dependencyFunc();
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (myChoice != Entity.Exit);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    /// <summary>
    /// Manages user interactions for employee-related operations, including creating, reading, updating, and deleting employees.
    /// </summary>
    private static void employeeFunc()
    {
        CRUD myChoice;

        // Continue displaying the menu and processing user choices until the user chooses to exit
        do
        {
            // Display the CRUD menu for user selection
            displayMenu();

            // Get user input and parse it into the CRUD enum
            if (!Enum.TryParse(Console.ReadLine(), out myChoice))
                throw new Exception("INVALID VALUE");

            // Switch based on the user's CRUD choice
            switch (myChoice)
            {
                case CRUD.Create:
                    // Prompt the user to enter details for creating a new employee
                    Console.WriteLine("Enter the worker's ID");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                        throw new Exception("INVALID VALUE");

                    Console.WriteLine("Enter the worker's name");
                    string? name = Console.ReadLine() ?? throw new Exception("Wrong input");

                    Console.WriteLine("Enter the worker's hourly rate");
                    if (!int.TryParse(Console.ReadLine(), out int hourlyRate))
                        throw new Exception("INVALID VALUE");

                    Console.WriteLine("Enter the worker's email");
                    string? email = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    // Generate random values for work status and complexity if not provided by the user
                    WorkStatus status = (WorkStatus)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 3)}");
                    Type complexity = (Type)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");

                    // Create a new employee with the provided details and add it to the system
                    s_dal.Employee!.Create(new Employee(id, name, email, hourlyRate, status, complexity));
                    break;

                case CRUD.Read:
                    // Prompt the user to enter the ID of the employee for reading
                    Console.WriteLine("Enter Employee ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int ID))
                        throw new Exception("INVALID VALUE");

                    // Read and display the specified employee
                    Employee? readEmployee = s_dal!.Employee.Read(ID);
                    Console.WriteLine(readEmployee);
                    break;

                case CRUD.ReadAll:
                    // Display all employees in the system
                    foreach (var worker in s_dal!.Employee.ReadAll())
                    {
                        Console.WriteLine(worker);
                    }
                    break;

                case CRUD.Update:
                    // Prompt the user to enter the ID of the employee for updating
                    Console.WriteLine("Enter Employee ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int iD))
                        throw new Exception("INVALID VALUE");

                    // Read and display the existing employee to get its details
                    Employee updatedEmployee = s_dal!.Employee.Read(iD)! ?? throw new Exception($"Can't update, worker does not exist!!");
                    Console.WriteLine(updatedEmployee);

                    // Prompt the user to enter updated details for the employee
                    Console.WriteLine("Enter the worker's ID");
                    if (!int.TryParse(Console.ReadLine(), out int _id))
                        throw new Exception("INVALID VALUE");

                    Console.WriteLine("Enter the worker's name");
                    string? _name = Console.ReadLine() ?? throw new Exception("Wrong input");

                    Console.WriteLine("Enter the worker's hourly rate");
                    if (!int.TryParse(Console.ReadLine(), out int _hourlyRate))
                        throw new Exception("INVALID VALUE");

                    Console.WriteLine("Enter the worker's email");
                    string? _email = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    // Generate random values for work status and complexity if not provided by the user
                    WorkStatus _status = (WorkStatus)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 3)}");
                    Type _complexity = (Type)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");

                    // Create a new Employee object with updated details and update it in the system
                    Employee employee = new Employee(_id, _name, _email, _hourlyRate, _status, _complexity);
                    s_dal!.Employee.Update(employee);
                    break;

                case CRUD.Delete:
                    // Prompt the user to enter the ID of the employee for deletion
                    Console.WriteLine("Enter worker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int Id))
                        throw new Exception("Invalid input");

                    // Delete the specified employee
                    s_dal!.Employee.Delete(Id);
                    break;

                case CRUD.Exit:
                    // Inform the user that the program is exiting
                    Console.WriteLine("Exiting program");
                    break;

                default:
                    // Inform the user of an invalid choice and prompt for a valid option
                    Console.WriteLine("ERROR");
                    break;
            }

        } while (myChoice != CRUD.Exit);
    }

    /// <summary>
    /// Manages user interactions for task-related operations, including creating, reading, updating, and deleting tasks.
    /// </summary>
    private static void taskFunc()
    {
        CRUD myChoice;

        // Continue displaying the menu and processing user choices until the user chooses to exit
        do
        {
            // Display the CRUD menu for user selection
            displayMenu();

            // Get user input and parse it into the CRUD enum
            if (!Enum.TryParse(Console.ReadLine(), out myChoice))
                throw new Exception("INVALID VALUE");

            // Switch based on the user's CRUD choice
            switch (myChoice)
            {
                case CRUD.Create:
                    // Prompt the user to enter details for creating a new task
                    Console.WriteLine("enter the Name of the task: ");
                    string? name = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    // Prompt the user for task details such as description, remarks, result product, etc.
                    // Additional error handling is included to handle incorrect user inputs
                    Console.WriteLine("enter Description of the task: ");
                    string? Description = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter Remarks of the task: ");
                    string? Remarks = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter ResultProduct of the task: ");
                    string? ResultProduct = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter id of the employee: ");
                    if (!int.TryParse(Console.ReadLine(), out int IdWorker))
                        throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter level of the employee: ");
                    Type complexity = (Type)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");

                    // Prompt the user for additional task details, such as milestone, dates, and deadlines
                    Console.WriteLine("enter milestone-False/True : ");
                    if (!bool.TryParse(Console.ReadLine(), out bool milestone))
                        throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter the date the task was created");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime createAtDate))
                        throw new Exception("the date is not correct");

                    Console.WriteLine("Enter the time needed to complete the task");
                    if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan requiredEffortTime))
                        throw new Exception("the time is not correct");

                    Console.WriteLine("Enter the date you started working on the task ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                        throw new Exception("the date is not correct");

                    Console.WriteLine("Enter the estimated end date");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime scheduledDate))
                        throw new Exception("the date is not correct");

                    Console.WriteLine("Press the DEADLINE to complete the task");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime DeadLine))
                        throw new Exception("the date is not correct");

                    Console.WriteLine("Enter the actual end date");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime completeDate))
                        throw new Exception("the date is not correct");

                    // Create a new task with the provided details and add it to the system
                    s_dal.Task!.Create(new Task(0, IdWorker, name, Description, createAtDate, requiredEffortTime, milestone,
                        complexity, startDate, scheduledDate, DeadLine, completeDate, ResultProduct, Remarks));
                    break;

                case CRUD.Read:
                    // Prompt the user to enter the ID of the task for reading
                    Console.WriteLine("Enter the ID of the task: ");
                    if (!int.TryParse(Console.ReadLine(), out int ID))
                        throw new Exception("INVALID VALUE");

                    // Read and display the specified task
                    Task? readTask = s_dal.Task!.Read(ID);
                    Console.WriteLine(readTask);
                    break;

                case CRUD.ReadAll:
                    // Display all tasks in the system
                    Console.WriteLine("List of the Tasks: ");
                    foreach (var item in s_dal.Task!.ReadAll())
                        Console.WriteLine(item);
                    break;

                case CRUD.Update:
                    // Prompt the user to enter the ID of the task for updating
                    Console.WriteLine("Enter Assignments Id: ");
                    if (!int.TryParse(Console.ReadLine(), out int Id))
                        throw new Exception("INVALID VALUE");

                    // Read and display the existing task to get its details
                    Console.WriteLine(s_dal.Task!.Read(Id));

                    // Prompt the user to enter updated details for the task
                    // Additional error handling is included for incorrect user inputs
                    Console.WriteLine("enter the Name of the task: ");
                    string? _name = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter Description of the task: ");
                    string? _Description = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter Remarks of the task: ");
                    string? _Remarks = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter ResultProduct of the task: ");
                    string? _ResultProduct = Console.ReadLine() ?? throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter id of the employee: ");
                    if (!int.TryParse(Console.ReadLine(), out int _IdWorker))
                        throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter level of the employee: ");
                    Type _complexity = (Type)int.Parse(Console.ReadLine() ?? $"{s_rand.Next(0, 5)}");

                    Console.WriteLine("enter milestone-False/True : ");
                    if (!bool.TryParse(Console.ReadLine(), out bool _milestone))
                        throw new Exception("INVALID VALUE");

                    Console.WriteLine("enter the date the task was created");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime _createAtDate))
                        throw new Exception("the date is not correct");

                    Console.WriteLine("Enter the time needed to complete the task");
                    if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan _requiredEffortTime))
                        throw new Exception("the time is not correct");

                    Console.WriteLine("Enter the date you started working on the task ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime _startDate))
                        throw new Exception("the date is not correct");

                    Console.WriteLine("Enter the estimated end date");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime _scheduledDate))
                        throw new Exception("the date is not correct");

                    Console.WriteLine("Press the DEADLINE to complete the task");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime _DeadLine))
                        throw new Exception("the date is not correct");

                    Console.WriteLine("Enter the actual end date");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime _completeDate))
                        throw new Exception("the date is not correct");

                    // Create a new Task object with updated details and update it in the system
                    Task updatedTask = new Task(Id, _IdWorker, _name, _Description, _createAtDate, _requiredEffortTime, _milestone,
                        _complexity, _startDate, _scheduledDate, _DeadLine, _completeDate, _ResultProduct, _Remarks);
                    s_dal.Task!.Update(updatedTask);
                    break;

                case CRUD.Delete:
                    // Prompt the user to enter the ID of the task for deletion
                    Console.WriteLine("Enter the ID of the task: ");
                    if (!int.TryParse(Console.ReadLine(), out int idDelete))
                        throw new Exception("INVALID VALUE");

                    // Delete the specified task
                    s_dal.Task!.Delete(idDelete);
                    break;

                case CRUD.Exit:
                    // Inform the user that the program is exiting
                    Console.WriteLine("Exiting program");
                    break;

                default:
                    // Inform the user of an invalid choice and prompt for a valid option
                    Console.WriteLine("Invalid choice! Please enter a valid option");
                    break;
            }

        } while (myChoice != CRUD.Exit); // Continue the loop until the user chooses to exit
    }


    /// <summary>
    /// Manages user interactions for dependency operations, including creating, reading, updating, and deleting dependencies.
    /// Also provides the functionality to read the dependency between two tasks.
    /// </summary>
    private static void dependencyFunc()
    {
        CRUD myChoice;

        // Continue displaying the menu and processing user choices until the user chooses to exit
        do
        {
            // Display the CRUD menu for user selection
            displayMenu();

            // Additional menu option for reading the dependency between two tasks
            Console.WriteLine("6-Read Dependency Of Two Tasks");

            // Get user input and parse it into the CRUD enum
            if (!Enum.TryParse(Console.ReadLine(), out myChoice))
                throw new Exception("INVALID VALUE");

            // Switch based on the user's CRUD choice
            switch (myChoice)
            {
                case CRUD.Create:
                    // Prompt the user to enter task IDs for creating a dependency
                    int task2Id, task1Id;
                    Console.WriteLine("Enter TasksIDs of the dependency:");
                    if (!int.TryParse(Console.ReadLine(), out task1Id))
                        throw new Exception("INVALID VALUE");
                    Console.WriteLine("Enter TasksIDs of the dependency:");
                    if (!int.TryParse(Console.ReadLine(), out task2Id))
                        throw new Exception("INVALID VALUE");

                    // Create a new dependency using the provided task IDs
                    s_dal!.Dependency.Create(new Dependency(task1Id, task2Id));
                    break;

                case CRUD.Read:
                    // Prompt the user to enter a dependency ID for reading
                    Console.WriteLine("Enter a dependency ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int readId))
                        throw new Exception("INVALID VALUE");

                    // Read and display the specified dependency
                    Dependency? readDependency = s_dal!.Dependency.Read(readId);
                    Console.WriteLine(readDependency is null ? "Link was not found!\n" : readDependency);
                    break;

                case CRUD.ReadAll:
                    // Display all dependencies in the system
                    Console.WriteLine("The list of the Dependencies: ");
                    foreach (var item in s_dal!.Dependency.ReadAll())
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case CRUD.Update:
                    // Prompt the user to enter details for updating a dependency
                    Console.WriteLine("Enter the requested link number, and two updated task codes:");
                    if (!int.TryParse(Console.ReadLine(), out int updatedId))
                        throw new Exception("INVALID VALUE");

                    // Read the existing dependency to get its details
                    Dependency? outOfDateDependency = s_dal!.Dependency.Read(updatedId) ?? throw new Exception("INVALID VALUE");
                    Console.WriteLine(outOfDateDependency);

                    // Prompt the user to enter updated task codes
                    Console.WriteLine("Enter the two updated tasks codes:");
                    if (!int.TryParse(Console.ReadLine(), out int task1))
                        throw new Exception("INVALID VALUE");
                    if (!int.TryParse(Console.ReadLine(), out int task2))
                        throw new Exception("INVALID VALUE");

                    // Create a new Dependency object with updated details and update it in the system
                    Dependency updatedDependency = new Dependency(updatedId, task1, task2);
                    s_dal!.Dependency.Update(updatedDependency);
                    break;

                case CRUD.Delete:
                    // Prompt the user to enter the ID of the dependency to delete
                    Console.WriteLine("Enter the ID of the dependency: ");
                    if (!int.TryParse(Console.ReadLine(), out int idDelete))
                        throw new Exception("INVALID VALUE");

                    // Delete the specified dependency
                    s_dal!.Dependency.Delete(idDelete);
                    break;

                case CRUD.ReadDependencyOfTwoTasks:
                    // Prompt the user to enter task IDs for reading the dependency between two tasks
                    Console.WriteLine("Enter TasksIDs of the first task");
                    if (!int.TryParse(Console.ReadLine(), out task1))
                        throw new Exception("INVALID VALUE");
                    Console.WriteLine("Enter TasksIDs of the second task:");
                    if (!int.TryParse(Console.ReadLine(), out task2))
                        throw new Exception("INVALID VALUE");

                    // Read and display the dependency between the specified two tasks
                    s_dal!.Dependency.Read(task1, task2);
                    break;

                case CRUD.Exit:
                    // Inform the user that the program is exiting
                    Console.WriteLine("Exiting program");
                    break;

                default:
                    // Inform the user of an invalid choice and prompt for a valid option
                    Console.WriteLine("Invalid choice! Please enter a valid option");
                    break;
            }

        } while (myChoice != CRUD.Exit); // Continue the loop until the user chooses to exit
    }


    /// <summary>
    /// Displays the menu for CRUD (Create, Read, Update, Delete) operations.
    /// </summary>
    private static void displayMenu()
    {
        // Prompt the user to choose an operation
        Console.WriteLine("Choose operation:");

        // Iterate through each CRUD operation in the CRUD enum
        foreach (CRUD option in Enum.GetValues(typeof(CRUD)))
        {
            // Display the numeric value and lowercase string representation of each CRUD operation
            Console.WriteLine($"{(int)option} - {option.ToString().ToLower()}");
        }

        // Display the option to exit the menu
        Console.WriteLine("0 - Exit");
    }



}



