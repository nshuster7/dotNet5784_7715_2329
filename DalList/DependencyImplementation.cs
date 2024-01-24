namespace Dal
{
    using DalApi;
    using DO;
    using System.Collections.Generic;
    //   using System.Runtime.Intrinsics.Arm;

    /// <summary>
    /// Implementation of the IDependency interface for handling dependencies.
    /// </summary>
    internal class DependencyImplementation : IDependency
    {
        /// <summary>
        /// Creates a new dependency and adds it to the data source.
        /// </summary>
        /// <param name="dep">The dependency to be created.</param>
        /// <returns>The ID of the newly created dependency.</returns>
        public int Create(Dependency dep)
        {
            int newNum = DataSource.Config.NextDependencyId;
            Dependency newDep = dep with { Id = newNum };
            DataSource.Dependencies.Add(newDep);
            return newNum;
        }

        /// <summary>
        /// Deletes a dependency based on its ID.
        /// </summary>
        /// <param name="id">The ID of the dependency to be deleted.</param>
        /// <exception cref="DalDoesNotExistException">Thrown if the dependency does not exist.</exception>
        public void Delete(int id)
        {
            if (Read(id) == null)
            {
                throw new DalDoesNotExistException($"Dependency with ID={id} does NOT exist");
            }
            else
                DataSource.Dependencies.RemoveAt(id);
        }

        /// <summary>
        /// Checks if one task depends on another.
        /// </summary>
        /// <param name="IdTask1">ID of the dependent task.</param>
        /// <param name="IdTask2">ID of the task being depended upon.</param>
        /// <returns>Returns the dependency if task1 depends on task2.</returns>
        public Dependency? Check(int IdTask1, int IdTask2)
        {
            return DataSource.Dependencies.FirstOrDefault(dep => dep.DependentTask == IdTask1 && dep.DependsOnTask == IdTask2);
        }

        /// <summary>
        /// Reads a dependency based on its ID.
        /// </summary>
        /// <param name="IdDependence">The ID of the dependency to be read.</param>
        /// <returns>The dependency with the specified ID, if it exists.</returns>
        public Dependency? Read(int IdDependence)
        {
            return DataSource.Dependencies.FirstOrDefault(dep => dep.Id == IdDependence);
        }

        /// <summary>
        /// Reads a dependency based on a custom filter function.
        /// </summary>
        /// <param name="filter">The filter function for selecting a dependency.</param>
        /// <returns>The first dependency that satisfies the filter, if any.</returns>
        public Dependency? Read(Func<Dependency, bool> filter)  // Stage 2
        {
            return DataSource.Dependencies.FirstOrDefault(filter);
        }

        /// <summary>
        /// Reads all dependencies.
        /// </summary>
        /// <returns>A list containing all dependencies in the data source.</returns>
        public List<Dependency> ReadAll()
        {
            return new List<Dependency>(DataSource.Dependencies);
        }

        /// <summary>
        /// Reads all dependencies based on a custom filter function.
        /// </summary>
        /// <param name="filter">The filter function for selecting dependencies (optional).</param>
        /// <returns>An IEnumerable containing dependencies that satisfy the filter, or all dependencies if no filter is provided.</returns>
        public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) // Stage 2
        {
            if (filter != null)
            {
                return from item in DataSource.Dependencies
                       where filter(item)
                       select item;
            }
            return from item in DataSource.Dependencies
                   select item;
        }

        /// <summary>
        /// Updates an existing dependency.
        /// </summary>
        /// <param name="dep">The dependency with updated information.</param>
        /// <exception cref="DalDoesNotExistException">Thrown if the dependency does not exist.</exception>
        public void Update(Dependency dep)
        {
            if (Read(dep.Id) == null)
            {
                throw new DalDoesNotExistException($"Dependency with ID={dep.Id} does NOT exist");
            }
            Delete(dep.Id);
            DataSource.Dependencies.Add(dep);
        }

        /// <summary>
        /// Empty the Dependencies list of data
        /// </summary>
        public void Clear()
        {
            DataSource.Dependencies.Clear();
        }
    }
}

