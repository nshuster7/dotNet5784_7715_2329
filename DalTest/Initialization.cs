

namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{
    private static IEmployee? s_dalEmployee; //stage 1
    private static ITask? s_dalTask; //stage 1
    private static IDependency? s_dalDependency; //stage 1
    private static readonly Random s_rand = new();

    private static void createDependency()
    { }
    private static void createTask()
    {
        // 1. Define variables
        const int MIN_TASK_ID = 1;
        const int MAX_TASK_ID = 1000;


        // 2. Create an array of random task names
        string[] taskNames = new string[] {
        "Develop new feature",
        "Fix bug",
        "Add documentation",
        "Test new feature",
        "Deploy new feature",
        "Release new version"
    };

        // 3. Create an array of descriptions for the tasks
        string[] taskDescriptions = new string[] {
        "Develop a new feature for the product.",
        "Fix a bug in the product.",
        "Add documentation for the product.",
        "Test a new feature for the product.",
        "Deploy a new feature to production.",
        "Release a new version of the product."
    };

        // 4. Create a loop to populate the data
        for (int i = 0; i < 25; i++)
        {
            // 5. Generate a random ID
            int id = s_rand.Next(MIN_TASK_ID, MAX_TASK_ID);

            // 6. Generate a random name
            string name = taskNames[i];

            // 7. Generate a random complexity
            Type complexity = (Type)s_rand.Next(0,5);

            // 8. Generate a random date
            DateTime createdAtDate = DateTime.Now.AddDays(-s_rand.Next(1, 30));

            // 9. Generate a random description
            string description = taskDescriptions[i];

            // 10. Create a new task object
            Task task = new Task(
                id,
                null,
                name,
                description,
                createdAtDate,
                null,
                false,
                complexity,
                null,
                null,
                null,
                null,
                null,
                null
            );

            // 11. Add the object to the list
            s_dalTask!.Create(task);
        }
    }

    private static void createEmployee()
    {

        // Define variables
        const int MIN_ID = 100000000;
        const int MAX_ID = 999999999;
        const int MIN_HOURLY_RATE = 400;
        const int MAX_HOURLY_RATE = 1200;

        //Create an array of logical employee names
        string[] employeeNames = new string[] {
    "Ali Al-Aziz",
    "Ahmed Al-Hamad",
    "Mohammed Al-Hussein",
    "Hassan Al-Jawad",
    "Omar Al-Mukhtar",
    "Mahmoud Al-Sharif",
    "Khaled Al-Wazir",
    "Yousef Al-Zahrani",
    "Ibrahim Al-Qadi",
    "Li Jian",
    "Wang Wei",
    "Zhang Xiao",
    "Zhao Hua",
    "Wu Li",
    "Liu Yan",
    "Yang Jie",
    "Huang Yu",
    "Ram Kumar",
    "Krishna Sharma",
    "Rahul Gupta",
    "Amit Singh",
    "Arjun Sharma",
    "Ravi Kumar",
    "David Levi",
    "Moshe Baruch",
    "Abraham Cohen",
};


        // Create a loop to populate the data
        for (int i = 0; i < 25; i++)
        {
            // Generate a random ID
            int id = s_rand.Next(MIN_ID, MAX_ID);

            // Generate a random name
            string name = employeeNames[i];

            // Generate a random email address
            string? email = null;
            if (s_rand.Next(0, 2) == 0) //Not every Employee has an email
            {
                email = $"{name}@example.com";
            }

            // Generate a random hourly rate
            int hourlyRate = 0;
            Type type = (Type)s_rand.Next(0, 5);
            switch (type)
            {
                case Type.Beginner:
                    hourlyRate = MIN_HOURLY_RATE;
                    break;
                case Type.AdvancedBeginner:
                    hourlyRate = MIN_HOURLY_RATE + 100;
                    break;
                case Type.Intermediate:
                    hourlyRate = MIN_HOURLY_RATE + 200;
                    break;
                case Type.Advanced:
                    hourlyRate = MIN_HOURLY_RATE + 300;
                    break;
                case Type.Expert:
                    hourlyRate = MAX_HOURLY_RATE;
                    break;
            }

            // Generate a random work status
            WorkStatus workStatus = (WorkStatus)s_rand.Next(0, 3);

            // Create a new object
            Employee employee = new Employee(
                id,
                name,
                email,
                hourlyRate,
                workStatus,
                type
            );

            // Add the object to the list
            if (s_dalEmployee!.Read(employee.Id) == null)
                s_dalEmployee!.Create(employee);
        }
    }
}
