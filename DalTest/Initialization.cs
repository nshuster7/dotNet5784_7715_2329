namespace DalTest;
using DalApi;
using DO;
using System;
using System.Security.Cryptography;

public static class Initialization
{
    //private static IEmployee? s_dalEmployee; //stage 1
    //private static ITask? s_dalTask; //stage 1
    //private static IDependency? s_dalDependency; //stage 1
    private static IDal? s_dal;
    private static readonly Random s_rand = new();
    private static void createUsers()
    {
        // Get all employees
        IEnumerable<Employee> allEmployees = s_dal!.Employee.ReadAll()!;

        // Choose randomly 15 employees
        IEnumerable<Employee> selectedEmployees = allEmployees.Take(15);

        // Iterate over selected employees to create users
        foreach (var employee in selectedEmployees)
        {
            // Use the employee's ID as the unique identifier for the user
            int userId = employee.Id;

            // Use the employee's name as the username for the user
            string username = employee.Name;

            // Generate a random password for the user
            string password = GenerateRandomPassword();

            // Create the user object
            User user = new User(
                userId,
                username,
                password,
                false
            );

            // Add the user to the data source
            s_dal.User.Create(user);
        }
        var emp1 = s_dal.Employee.Read(e => e.Name == "Moshe Baruch");
        User manager1 = new User(emp1!.Id, "Moshe Baruch","12345678",true);
        var emp2 = s_dal.Employee.Read(e => e.Name == "David Levi");
        User manager2 = new User(emp2!.Id, "David Levi", "87654321", true);
        var emp3 = s_dal.Employee.Read(e => e.Name == "Moshe Baruch");
        User empUser1 = new User(emp1!.Id, "Amit Singh", "11223344", false);
        var emp4 = s_dal.Employee.Read(e => e.Name == "David Levi");
        User empUser2 = new User(emp2!.Id, "Ravi Kumar", "44332211", false);
        s_dal.User.Create(manager1!);
        s_dal.User.Create(manager2!);
        s_dal.User.Create(empUser1!);
        s_dal.User.Create(empUser2!);
    }

    
    private static void createDependency()
    {
        s_dal!.Dependency.Create(new Dependency(0, 2, 1));
        s_dal!.Dependency.Create(new Dependency(0, 2, 1));
        s_dal!.Dependency.Create(new Dependency(0, 3, 2));
        s_dal!.Dependency.Create(new Dependency(0, 4, 3));
        s_dal!.Dependency.Create(new Dependency(0, 5, 4));
        s_dal!.Dependency.Create(new Dependency(0, 6, 5));
        s_dal!.Dependency.Create(new Dependency(0, 7, 6));
        s_dal!.Dependency.Create(new Dependency(0, 8, 7));
        s_dal!.Dependency.Create(new Dependency(0, 9, 7));
        s_dal!.Dependency.Create(new Dependency(0, 10, 9));
        s_dal!.Dependency.Create(new Dependency(0, 11, 9));
        s_dal!.Dependency.Create(new Dependency(0, 12, 11));
        s_dal!.Dependency.Create(new Dependency(0, 13, 12));
        s_dal!.Dependency.Create(new Dependency(0, 14, 12));
        s_dal!.Dependency.Create(new Dependency(0, 15, 13));
        s_dal!.Dependency.Create(new Dependency(0, 15, 14));
        s_dal!.Dependency.Create(new Dependency(0, 16, 12));
        s_dal!.Dependency.Create(new Dependency(0, 17, 12));
        s_dal!.Dependency.Create(new Dependency(0, 18, 14));
        s_dal!.Dependency.Create(new Dependency(0, 19, 14));
        s_dal!.Dependency.Create(new Dependency(0, 20, 13));
        s_dal!.Dependency.Create(new Dependency(0, 20, 19));
        s_dal!.Dependency.Create(new Dependency(0, 21, 20));
        s_dal!.Dependency.Create(new Dependency(0, 22, 21));
        s_dal!.Dependency.Create(new Dependency(0, 23, 16));
        s_dal!.Dependency.Create(new Dependency(0, 23, 17));
        s_dal!.Dependency.Create(new Dependency(0, 24, 19));
        s_dal!.Dependency.Create(new Dependency(0, 24, 21));
        s_dal!.Dependency.Create(new Dependency(0, 25, 24));
        s_dal!.Dependency.Create(new Dependency(0, 25, 19));
        s_dal!.Dependency.Create(new Dependency(0, 26, 24));
        s_dal!.Dependency.Create(new Dependency(0, 26, 19));
        s_dal!.Dependency.Create(new Dependency(0, 27, 23));
        s_dal!.Dependency.Create(new Dependency(0, 27, 26));
        s_dal!.Dependency.Create(new Dependency(0, 28, 11));
        s_dal!.Dependency.Create(new Dependency(0, 29, 28));
        s_dal!.Dependency.Create(new Dependency(0, 30, 28));
        s_dal!.Dependency.Create(new Dependency(0, 31, 28));
        s_dal!.Dependency.Create(new Dependency(0, 32, 28));
        s_dal!.Dependency.Create(new Dependency(0, 33, 25));
        s_dal!.Dependency.Create(new Dependency(0, 33, 32));
        s_dal!.Dependency.Create(new Dependency(0, 34, 27));
        s_dal!.Dependency.Create(new Dependency(0, 34, 33));
        s_dal!.Dependency.Create(new Dependency(0, 35, 34));
        s_dal!.Dependency.Create(new Dependency(0, 36, 25));
        s_dal!.Dependency.Create(new Dependency(0, 36, 26));
        s_dal!.Dependency.Create(new Dependency(0, 37, 36));
        s_dal!.Dependency.Create(new Dependency(0, 38, 23));
        s_dal!.Dependency.Create(new Dependency(0, 38, 27));
        s_dal!.Dependency.Create(new Dependency(0, 39, 35));
        s_dal!.Dependency.Create(new Dependency(0, 39, 38));
        s_dal!.Dependency.Create(new Dependency(0, 40, 39));
    }
    private static void createTask()
    {
        //Create an array of random task names
        string[] taskNames = new string[] {
            "null", "COSAS", "EOAP", "OTNPAP", "DSEP", "PDCE", "FOFI",
            "POBM", "ETCS", "EAPI", "IOS", "CCF", "BSF", "IWAI",
            "CEWAF","IWAD", "PAEW", "HVACSI", "IOWAC", "IWAIP",
            "CFAT", "AIF", "IOD", "TPEC", "ICAC", "FAID", "IOEA",
            "PRT", "AEF", "GAOD", "IFAG", "PPAB", "IOEL","DFC",
            "OCOO", "CFTAC", "COMM", "STEAS", "MFCPO","KDAO", "EAPCF"
           };

        //  Create an array of descriptions for the tasks
        string[] taskDescriptions = new string[] {
        "null",
        "Conduct survey and analysis of sites",
        "Engineering or architectural plans",
        "Obtain necessary permits and permissions",
        "Develop structural engineering plans",
        "Prepare detailed cost estimates",
        "Financing or financing insurance",
        "Purchase building materials",
        "Evacuation of the construction site",
        "Excavation and preparation of infrastructure",
        "Installation of services",
        "Casting concrete foundation",
        "Building a structural frame",
        "Installation of walls and insulation",
        "Completion of exterior walls and finishes",
        "Installation of windows and doors",
        "Plumbing and electrical work",
        "HVAC system installation",
        "Insulation of walls and ceilings",
        "Installation of interior walls and partitions",
        "Construction of flooring and tiles",
        "Apply internal finishes",
        "Installation of devices",
        "Termination of plumbing and electrical connections",
        "Installation of cabinets and countertops",
        "Full decoration and interior design",
        "Installation of electrical appliances",
        "Perform required tests",
        "Apply exterior finishes",
        "Gardening and outdoor devices",
        "Installation of fencing and gates",
        "Paving paths and bridges",
        "Installation of external lighting",
        "Do final cleaning",
        "Obtain certificate of occupancy",
        "Conduct final training and handover",
        "Prepare operating and maintenance manuals",
        "Staff training on equipment and systems",
        "Make final checks and place an order",
        "Key delivery and ownership",
        "Evaluate and analyze post-construction feedback"
    };

        //Create a loop to populate the data
        for (int i = 1; i <= 40; i++)
        {
            // Generate a random name
            string name = taskNames[i];
            // Generate a random description
            string description = taskDescriptions[i];
            // Generate a random complexity
            DO.Type complexity = (DO.Type)s_rand.Next(0, 5);
            int weeks = s_rand.Next(1, 13);
            TimeSpan duration = TimeSpan.FromDays(weeks * 7);
            // Create a new task object
            Task task = new Task(
                0,//the id is gonna to be changed by the function create
                0,
                name,
                description,
                 CreatedAtDate:DateTime.Now,
                 IsMilestone: false,
                 RequiredEffortTime: duration,
                Complexity: complexity
               
            );
            // 11. Add the object to the list
            s_dal!.Task.Create(task);
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
            int id;
            do
                // Generate a random ID
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dal!.Employee.Read(id) != null);
            // Generate a random name
            string name = employeeNames[i];

            // Generate a random email address
            string? email = null;

            if (s_rand.Next(0, 2) == 0) 
            {
                string sanitizedName = name.Replace(" ", ""); 
                email = $"{sanitizedName}@example.com";
            }
            // Generate a random hourly rate
            int hourlyRate = 0;
            DO.Type type = (DO.Type)s_rand.Next(0, 5);
            switch (type)
            {
                case DO.Type.Beginner:
                    hourlyRate = MIN_HOURLY_RATE;
                    break;
                case DO.Type.AdvancedBeginner:
                    hourlyRate = MIN_HOURLY_RATE + 100;
                    break;
                case DO.Type.Intermediate:
                    hourlyRate = MIN_HOURLY_RATE + 200;
                    break;
                case DO.Type.Advanced:
                    hourlyRate = MIN_HOURLY_RATE + 300;
                    break;
                case DO.Type.Expert:
                    hourlyRate = MAX_HOURLY_RATE;
                    break;
            }

            // Generate a random work status
            WorkStatus workStatus = (WorkStatus)s_rand.Next(0, 3);
            //string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent!.FullName;
            //string imagesPath = Path.Combine(projectDirectory, "PL", "Employee");
            //string imageName = $"{i + 1}.jpg";
            //string imagePath = Path.Combine(imagesPath, imageName);
            string imagePath = $"/Employee/{i + 1}.jpg";
            // Create a new object
            Employee employee = new Employee(
                id,
                name,
                email,
                hourlyRate,
                workStatus,
                type,
                imagePath

            );
            // Add the object to the list
            s_dal.Employee.Create(employee);
        }
    }

    //public static void Do(IEmployee? dalEmployee, ITask? dalTask, IDependency? dalDependency)
    //public static void Do(IDal dal) //stage 2
    public static void Do() //stage 4
    {
        //s_dalEmployee = dalEmployee ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = Factory.Get; //stage 4

        createEmployee();
        createTask();
        createDependency();
        createUsers();
    }

    private static string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[s_rand.Next(s.Length)]).ToArray());
    }

    public static void Reset()
    {
        s_dal = Factory.Get;
        s_dal!.Employee.Clear();
        s_dal!.Task.Clear();
        s_dal!.Dependency.Clear();
        s_dal!.User.Clear();
        s_dal!.startProjectDate = null;
    }
}
