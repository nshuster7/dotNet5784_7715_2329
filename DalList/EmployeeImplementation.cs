namespace Dal
{
    using DalApi;
    using DO;
    using System.Collections.Generic;

    /// <summary>
    /// Implementation of the IEmployee interface for handling employees.
    /// </summary>
    internal class EmployeeImplementation : IEmployee
    {
        /// <summary>
        /// Empty the employee list of data
        /// </summary>
        public void Clear()
        {
            DataSource.Employees.Clear();
        }

        /// <summary>
        /// Creates a new employee and adds it to the data source.
        /// </summary>
        /// <param name="emp">The employee to be created.</param>
        /// <returns>The ID of the newly created employee.</returns>
        /// <exception cref="DalAlreadyExistsException">Thrown if the employee with the same ID already exists.</exception>
        public int Create(Employee emp)
        {
            if (Read(emp.Id) == null)
            {
                DataSource.Employees.Add(emp);
                return emp.Id;
            }
            throw new DalAlreadyExistsException($"Worker with ID={emp.Id} already exists");
        }

        /// <summary>
        /// Deletes an employee based on its ID.
        /// </summary>
        /// <param name="id">The ID of the employee to be deleted.</param>
        /// <exception cref="DalDoesNotExistException">Thrown if the employee does not exist.</exception>
        public void Delete(int id)
        {
            if (Read(id) == null)
                throw new DalDoesNotExistException($"Worker with ID={id} does NOT exist");
            else
                DataSource.Employees.RemoveAt(id);
        }

        /// <summary>
        /// Reads an employee based on its ID.
        /// </summary>
        /// <param name="id">The ID of the employee to be read.</param>
        /// <returns>The employee with the specified ID, if it exists.</returns>
        public Employee? Read(int id)
        {
            return DataSource.Employees.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Reads an employee based on a custom filter function.
        /// </summary>
        /// <param name="filter">The filter function for selecting an employee.</param>
        /// <returns>The first employee that satisfies the filter, if any.</returns>
        public Employee? Read(Func<Employee, bool> filter)  // Stage 2
        {
            return DataSource.Employees.FirstOrDefault(filter);
        }

        /// <summary>
        /// Reads all employees.
        /// </summary>
        /// <returns>A list containing all employees in the data source.</returns>
        public List<Employee> ReadAll()
        {
            return new List<Employee>(DataSource.Employees);
        }

        /// <summary>
        /// Reads all employees based on a custom filter function.
        /// </summary>
        /// <param name="filter">The filter function for selecting employees (optional).</param>
        /// <returns>An IEnumerable containing employees that satisfy the filter, or all employees if no filter is provided.</returns>
        public IEnumerable<Employee> ReadAll(Func<Employee, bool>? filter = null) // Stage 2
        {
            if (filter != null)
            {
                return from item in DataSource.Employees
                       where filter(item)
                       select item;
            }
            return from item in DataSource.Employees
                   select item;
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="emp">The employee with updated information.</param>
        /// <exception cref="DalDoesNotExistException">Thrown if the employee does not exist.</exception>
        public void Update(Employee emp)
        {
            if (Read(emp.Id) == null)
                throw new DalDoesNotExistException($"Worker with ID={emp.Id} does NOT exist");
            Delete(emp.Id);
            DataSource.Employees.Add(emp);
        }
    }
}

