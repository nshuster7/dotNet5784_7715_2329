using DalApi;
using DO;


namespace Dal;

internal class UserImplementation : IUser
{
  
    readonly string s_users_xml = "users";
    /// <summary>
    /// Adds a new user to the file of the users
    /// </summary>
    /// <param name="user"></param>
    /// <returns>int</returns>
    
    public int Create(DO.User user)
    {

        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        if (Read(user.ID) == null)
        {
            users.Add(user);
            XMLTools.SaveListToXMLSerializer(users, s_users_xml);
            return user.ID;

        }
        throw new DalAlreadyExistsException($"User with ID={user.ID} already exists");
    }

    /// <summary>
    /// Deletes an user from the file of users
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Delete(int id)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        if (Read(id) == null)
        {
            throw new DalDoesNotExistException($"User with ID={id} does NOT exist");
        }
        else
            users.RemoveAll(it => it.ID == id);
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
    }

    /// <summary>
    /// Update a user in the file of users
    /// </summary>
    /// <param name="t"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Update(DO.User user)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        if (Read(user.ID) == null)
            throw new DalDoesNotExistException($"User with ID={user.ID} does NOT exist");
        Delete(user.ID);
        users.Add(user);

        XMLTools.SaveListToXMLSerializer(users,s_users_xml);
    }

    /// <summary>
    /// Returns list of all the users, if gets a condition - by it
    /// </summary>
    /// <param name="func"></param>
    /// <returns>IEnumerable<DO.Order?></returns>
    public DO.User? Read(Func<DO.User, bool> filter)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        return users.FirstOrDefault(filter);
    }

    /// <summary>
    /// gets an ID and returns the user with this ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>DO.Order</returns>
    /// <exception cref="nullvalue"></exception>
    public DO.User? Read(int id)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        return users.FirstOrDefault(task => task.ID == id);
    }

    /// <summary>
    /// Gets a condition and returns a user with this condition
    /// </summary>
    /// <param name="func"></param>
    /// <returns>DO.User</returns>
    /// <exception cref="NotFound"></exception>
    public IEnumerable<DO.User?> ReadAll(Func<DO.User, bool>? filter = null)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        // Apply the filter if provided, otherwise return all tasks
        if (filter != null)
        {
            return from item in users
                   where filter(item)
                   select item;
        }
        return from item in users
               select item;
    }
    /// <summary>
    /// Empties the users file of data.
    /// </summary>
    public void Clear()
    {
        // Load the tasks from the XML file
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        // Clear the loaded list
        users.Clear();

        // Save the empty list back to the XML file
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
        Config.NextTaskId = 1;
    }
    public void ResetPassword(int id, string password)
    {
        DO.User user;
        DO.User? userRead = Read(id);
        if (userRead != null)
        {
            user = userRead with { Password = password };
            Update(user);
        }
        else
            throw new DalDoesNotExistException($"User with ID={id} does NOT exist");
    }
}
