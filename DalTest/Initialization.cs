

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
           "COSAS", "EOAP", "OTNPAP", "DSEP", "PDCE", "FOFI", 
            "POBM", "ETCS", "EAPI", "IOS", "CCF", "BSF", "IWAI",
            "CEWAF","IWAD", "PAEW", "HVACSI", "IOWAC", "IWAIP",
            "CFAT", "AIF", "IOD", "TPEC", "ICAC", "FAID", "IOEA",
            "PRT", "AEF", "GAOD", "IFAG", "PPAB", "IOEL","DFC", 
            "OCOO", "CFTAC", "COMM", "STEAS", "MFCPO","KDAO", "EAPCF"
           };

        //  Create an array of descriptions for the tasks
        string[] taskDescriptions = new string[] {
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
        bool alreadyExists=false;
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
            while (s_dalEmployee!.Read(id) == null);
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
                s_dalEmployee!.Create(employee);
        }
    }
}
