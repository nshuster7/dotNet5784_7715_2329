namespace BlApi;

public interface IEmployee
{
   
    /// <summary>
    /// Returns a list of all employees.
    /// </summary>
    /// <returns>A list of employees.</returns>
    IEnumerable<BO.Employee> ReadAll();

    /// <summary>
    /// Returns an employee by ID.
    /// </summary>
    /// <param name="id">The employee ID.</param>
    /// <returns>An employee or null if the employee does not exist.</returns>
    BO.Employee? Read(int id);

    /// <summary>
    /// Creates a new employee.
    /// </summary>
    /// <param name="employee">A new employee.</param>
    /// <returns>The ID of the new employee.</returns>
    int Create(BO.Employee employee);

    /// <summary>
    /// Updates an existing employee.
    /// </summary>
    /// <param name="employee">An existing employee.</param>
    void Update(BO.Employee employee);

    /// <summary>
    /// Deletes an existing employee.
    /// </summary>
    /// <param name="id">The employee ID.</param>
    void Delete(int id);

}
