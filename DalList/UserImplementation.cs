

namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
internal class UserImplementation : IUser
{
    /// <summary>
    /// adds the user to the users list and return it's id if doesn't already exist
    /// </summary>
    /// <param name="user"></param>
    /// <returns>int</returns>
    /// <exception cref="Exception"></exception>
    public int Create(User user)
    {
        if (Read(user.ID) == null)
        {
            DataSource.Users.Add(user);
            return user.ID;
        }
        throw new DalAlreadyExistsException($"User with ID={user.ID} already exists");
    }
    /// <summary>
    /// gets an ID and deletes the user with this ID
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        if (Read(id) == null)
            throw new DalDoesNotExistException($"User with ID={id} does NOT exist");
        else
            DataSource.Users.Remove(Read(id)!);
    }
    /// <summary>
    /// return the list of users
    /// </summary>
    /// <returns>List<Order></returns>
    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Users
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Users
               select item;
    }

    /// <summary>
    /// gets an id and return the user with this id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Product</returns>
    /// <exception cref="Exception"></exception>
    public User? Read(int id)
    {
        return DataSource.Users.FirstOrDefault(x => x.ID == id);
    }
    /// <summary>
    ///  returns user who meets the condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>User</returns>

    public User? Read(Func<User, bool> filter)  
    {
        return DataSource.Users.FirstOrDefault(filter);
    }

    public void ResetPassword(int id, string password)
    {
        User user;
        User? userRead = Read(id);
        if(userRead != null)
        {
            user= userRead with { Password = password };
            Update(user);
        }
        else
            throw new DalDoesNotExistException($"User with ID={id} does NOT exist");
    }

    /// <summary>
    /// gets a user and updetes it
    /// </summary>
    /// <param name="user"></param>
    public void Update(User user)
    {
        if (Read(user.ID) == null)
            throw new DalDoesNotExistException($"User with ID={user.ID} does NOT exist");
        Delete(user.ID );
        DataSource.Users.Add(user);
    }
    /// <summary>
    /// Empty the userers list of data
    /// </summary>
    public void Clear()
    {
        DataSource.Users.Clear();
    }
}
