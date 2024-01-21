using DalApi;
using DO;

namespace Dal;
using System.Xml.Linq;
internal class EmployeeImplementation: IEmployee
{
    readonly string s_employees_xml = "employees";

    public int Create(Employee item)
    {
        // Create an XElement representing the employee
        XElement employeeElem = new XElement("Employee",
            new XElement("Id", item.Id),
            new XElement("Name", item.Name),
            new XElement("Email", item.Email),
            new XElement("HourlyRate", item.HourlyRate),
            new XElement("WorkStatus", item.WorkStatus?.ToString()),
            new XElement("Type", item.type?.ToString())
        );

        // Add the employee element to the XML file
        XElement employeesRoot = XMLTools.LoadListFromXMLElement(s_employees_xml);
        employeesRoot.Add(employeeElem);
        XMLTools.SaveListToXMLElement(employeesRoot, s_employees_xml);

        return item.Id;
    }


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


    public Employee? Read(int id)
    {
        XElement employeesRoot = XMLTools.LoadListFromXMLElement(s_employees_xml);
        XElement? employeeElem = employeesRoot.Elements("Employee").FirstOrDefault(e => (int?)e.Element("Id") == id);
        return employeeElem != null ? getEmployee(employeeElem) : null;
    }

    static Employee getEmployee(XElement employeeElem)
    {
        return new Employee(
            employeeElem.ToIntNullable("Id") ?? throw new FormatException("Can't convert ID"),
            employeeElem.Element("Name")?.Value ?? "",
            employeeElem.Element("Email")?.Value,
            employeeElem.ToIntNullable("HourlyRate") ?? 0,
            (WorkStatus?)employeeElem.ToEnumNullable<WorkStatus>("WorkStatus"),
            (Type?)employeeElem.ToEnumNullable<Type>("Type")
        );
    }


    public Employee? Read(Func<Employee, bool> filter)
    {
        IEnumerable<Employee?> employees = ReadAll();
        return employees.FirstOrDefault(filter);
    }


    public IEnumerable<Employee?> ReadAll(Func<Employee, bool>? filter = null)
    {
        XElement employeesRoot = XMLTools.LoadListFromXMLElement(s_employees_xml);
        IEnumerable<XElement> employeeElems = employeesRoot.Elements("Employee");

        if (filter == null)
        {
            return employeeElems.Select(getEmployee);
        }
        else
        {
            return employeeElems.Select(getEmployee).Where(filter);
        }
    }



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
            employeeElem.SetElementValue("Type", item.type?.ToString());

            XMLTools.SaveListToXMLElement(employeesRoot, s_employees_xml);
        }

    }

}
