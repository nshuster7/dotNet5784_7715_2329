namespace Dal
{
    using DalApi;
    using DO;
    using System.Collections.Generic;

    /// <summary>
    /// Implementation of the ITask interface for handling tasks.
    /// </summary>
    internal class TaskImplementation : ITask
    {
        /// <summary>
        /// Empty the tasks list of data
        /// </summary>
        public void Clear()
        {
            DataSource.Tasks.Clear();
        }
        /// <summary>
        /// Creates a new task and adds it to the data source.
        /// </summary>
        /// <param name="item">The task to be created.</param>
        /// <returns>The ID of the newly created task.</returns>
        public int Create(Task item)
        {
            int newNum = DataSource.Config.NextTaskId;
            Task newTask = item with { Id = newNum };
            DataSource.Tasks.Add(newTask);
            return newNum;
        }

        /// <summary>
        /// Deletes a task based on its ID.
        /// </summary>
        /// <param name="id">The ID of the task to be deleted.</param>
        /// <exception cref="DalDoesNotExistException">Thrown if the task does not exist.</exception>
        public void Delete(int id)
        {
            if (Read(id) == null)
            {
                throw new DalDoesNotExistException($"Task with ID={id} does NOT exist");
            }
            else
                DataSource.Tasks.RemoveAt(id);
        }

        /// <summary>
        /// Reads a task based on its ID.
        /// </summary>
        /// <param name="id">The ID of the task to be read.</param>
        /// <returns>The task with the specified ID, if it exists.</returns>
        public Task? Read(int id)
        {
            return DataSource.Tasks.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Reads a task based on a custom filter function.
        /// </summary>
        /// <param name="filter">The filter function for selecting a task.</param>
        /// <returns>The first task that satisfies the filter, if any.</returns>
        public Task? Read(Func<Task, bool> filter)  // Stage 2
        {
            return DataSource.Tasks.FirstOrDefault(filter);
        }

        /// <summary>
        /// Reads all tasks.
        /// </summary>
        /// <returns>A list containing all tasks in the data source.</returns>
        public List<Task> ReadAll()
        {
            return new List<Task>(DataSource.Tasks);
        }

        /// <summary>
        /// Reads all tasks based on a custom filter function.
        /// </summary>
        /// <param name="filter">The filter function for selecting tasks (optional).</param>
        /// <returns>An IEnumerable containing tasks that satisfy the filter, or all tasks if no filter is provided.</returns>
        public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) // Stage 2
        {
            if (filter != null)
            {
                return from item in DataSource.Tasks
                       where filter(item)
                       select item;
            }
            return from item in DataSource.Tasks
                   select item;
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="item">The task with updated information.</param>
        /// <exception cref="DalDoesNotExistException">Thrown if the task does not exist.</exception>
        public void Update(Task item)
        {
            if (Read(item.Id) == null)
            {
                throw new DalDoesNotExistException($"Task with ID={item.Id} does NOT exist");
            }
            Delete(item.Id);
            DataSource.Tasks.Add(item);
        }

    }
}

