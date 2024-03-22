using DalApi;
using DO;
namespace Dal;
using System.Xml.Linq;
using Type = DO.Type;

internal class EmployeeImplementation : IEmployee
{
    readonly string s_employees_xml = "employees";
    /// <summary>
    /// Creates a new employee and adds it to the XML file.
    /// </summary>
    /// <param name="item">The employee object to be created.</param>
    /// <returns>The ID of the newly created employee.</returns>
    public int Create(Employee item)
    {
        // Create an XElement representing the employee
        XElement employeeElem;
        if (item.Email == null)
        {
            employeeElem = new XElement("Employee",
            new XElement("Id", item.Id),
            new XElement("Name", item.Name),
            new XElement("HourlyRate", item.HourlyRate),
            new XElement("WorkStatus", item.WorkStatus?.ToString()),
            new XElement("Type", item.Type?.ToString()),
            new XElement("ImageRelativeName", item.ImageRelativeName)
        );
        }
        else
        {
            employeeElem = new XElement("Employee",
            new XElement("Id", item.Id),
            new XElement("Name", item.Name),
            new XElement("Email", item.Email),
            new XElement("HourlyRate", item.HourlyRate),
            new XElement("WorkStatus", item.WorkStatus?.ToString()),
            new XElement("Type", item.Type?.ToString()),
            new XElement("ImageRelativeName", item.ImageRelativeName)
        );
        }
        // Add the employee element to the XML file
        XElement employeesRoot = XMLTools.LoadListFromXMLElement(s_employees_xml);
        employeesRoot.Add(employeeElem);
        XMLTools.SaveListToXMLElement(employeesRoot, s_employees_xml);
        return item.Id;
    }

    /// <summary>
    /// Deletes an employee from the XML file based on its ID.
    /// </summary>
    /// <param name="id">The ID of the employee to be deleted.</param>
    public void Delete(int id)
    {
        // Load the XML file and find the employee element to delete
        XElement employeesRoot = XMLTools.LoadListFromXMLElement(s_employees_xml);
        XElement? employeeElem = employeesRoot.Elements("Employee").FirstOrDefault(e => (int?)e.Element("Id") == id);

        // If the employee is found, remove it from the XML file
        if (employeeElem != null)
        {
            employeeElem.Remove();
            XMLTools.SaveListToXMLElement(employeesRoot, s_employees_xml);
        }
    }

    /// <summary>
    /// Reads an employee from the XML file based on its ID.
    /// </summary>
    /// <param name="id">The ID of the employee to be read.</param>
    /// <returns>The employee object if found, null otherwise.</returns>
    public Employee? Read(int id)
    {
        XElement employeesRoot = XMLTools.LoadListFromXMLElement(s_employees_xml);
        XElement? employeeElem = employeesRoot.Elements().FirstOrDefault(e => (int?)e.Element("Id") == id);
        return employeeElem != null ? getEmployee(employeeElem) : null;
    }

    /// <summary>
    /// Creates an Employee object from an XElement representing an employee.
    /// </summary>
    /// <param name="employeeElem">The XElement containing the employee data.</param>
    /// <returns>The created Employee object.</returns>
    /// <exception cref="FormatException">Thrown if there's an error parsing the XML data.</exception>
    static Employee getEmployee(XElement employeeElem)
    {
        return new Employee(
            employeeElem.ToIntNullable("Id") ?? throw new FormatException("Can't convert ID"),
            (string?)employeeElem.Element("Name") ?? "",
            (string?)employeeElem.Element("Email") ?? null,
            employeeElem.ToIntNullable("HourlyRate") ?? 0,
            (WorkStatus?)employeeElem.ToEnumNullable<WorkStatus>("WorkStatus"),
            (Type?)employeeElem.ToEnumNullable<Type>("Type"),
            (string?)employeeElem.Element("ImageRelativeName") ?? ""
        );
    }

    /// <summary>
    /// Reads the first employee from the XML file that matches the specified filter.
    /// </summary>
    /// <param name="filter">A function that determines if an employee matches the criteria.</param>
    /// <returns>The first employee object that matches the filter, null if none found.</returns>
    public Employee? Read(Func<Employee, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_employees_xml).Elements().Select(e => getEmployee(e)).FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all employees from the XML file, optionally applying a filter.
    /// </summary>
    /// <param name="filter">An optional function that filters the employees.</param>
    /// <returns>An enumerable collection of employee objects.</returns>
    public IEnumerable<Employee?> ReadAll(Func<Employee, bool>? filter = null)
    {
        XElement employeesRoot = XMLTools.LoadListFromXMLElement(s_employees_xml);
        IEnumerable<XElement> employeeElems = employeesRoot.Elements();

        if (filter == null)
        {
            return employeeElems.Select(getEmployee);
        }
        else
        {
            return employeeElems.Select(getEmployee).Where(filter);
        }
    }

    /// <summary>
    /// Updates an existing employee in the XML file.
    /// </summary>
    /// <param name="item">The updated employee object.</param>
    public void Update(Employee item)
    {
        // Load the XML file and find the employee element to update
        XElement employeesRoot = XMLTools.LoadListFromXMLElement(s_employees_xml);
        XElement? employeeElem = employeesRoot.Elements("Employee").FirstOrDefault(e => (int?)e.Element("Id") == item.Id);
        // If the employee is found, update its properties
        if (employeeElem != null)
        {
            employeeElem.SetElementValue("Name", item.Name);
            employeeElem.SetElementValue("Email", item.Email);
            employeeElem.SetElementValue("HourlyRate", item.HourlyRate);
            employeeElem.SetElementValue("WorkStatus", item.WorkStatus?.ToString());
            employeeElem.SetElementValue("Type", item.Type?.ToString());
            employeeElem.SetElementValue("ImageRelativeName", item.ImageRelativeName);
            XMLTools.SaveListToXMLElement(employeesRoot, s_employees_xml);
        }
    }
    /// <summary>
    /// Empty the employee file of data
    /// </summary>
    public void Clear()
    {

        // Create an empty XElement to represent the root of the XML file
        XElement employeesRoot = new XElement("Employees");

        // Save the empty root element to the XML file, effectively clearing it
        XMLTools.SaveListToXMLElement(employeesRoot, s_employees_xml);
    }
}
