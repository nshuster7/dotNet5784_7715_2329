using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bllmplementation;

internal class UserImplementation : BlApi.IUser
{
    /// <summary>
    /// access to the dal entities
    /// </summary>
    private DalApi.IDal dal = DalApi.Factory.Get;
    public void Create(BO.User user)
    {
        if (int.IsNegative(user.ID))//Checking if the ID is negative
            throw new BlWrongValueException("The task has WORNG VALUE!");

        // creates new DO user and copy into it the BO user's details
        DO.User dalUser = new DO.User(user.ID, user.Name, user.Password, user.IsManeger);

        // add the DO product to dal's products list
        try
        {
            dal.User.Create(dalUser);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={dalUser.ID} already exists", ex);
        }
    }

    public BO.User Read(int userID)
    {//Returns a user in Bo's view
        DO.User? user = dal.User.Read(userID);//call the reading function of the dal layer  

        if (user == null)
        {
            throw new BO.BlDoesNotExistException($"userID with ID={userID} does not exist");
        }

        return new BO.User() { ID=user.ID, Name=user.Name, Password=user.Password, IsManeger=user.IsManeger };
    }


    public IEnumerable<BO.User?> ReadAll(Func<DO.User, bool>? filter = null)
    {
        IEnumerable<DO.User?> doUser;
        if (filter is not null) //Check if a filter exist, and if so use it to read tasks from the DAL.
            doUser = dal.User.ReadAll(filter);
        else //If a filter doesn't exist, read all tasks from the DAL without a filter.
            doUser = dal.User.ReadAll();
        // Convert the list of DO tasks to a list of BO tasks.
        return (from user in doUser
                select new BO.User()
                {
                    ID = user.ID,
                    Name = user.Name,
                    Password = user.Password,
                    IsManeger = user.IsManeger
                });
    }
    public void Delete(int userId)
    {
        DO.Task? user = dal.Task.Read(userId);
        try
        {
            dal.Task.Delete(userId);
        }
        catch (DO.DalDoesNotExistException ex)
        {  // Throws an exception if the deletion fails.
            throw new BO.BlDoesNotExistException($"Failed to delete task from data layer: {ex.Message}", ex);
        }
    }
   public void ResetPassword(int ID, string password)
    {
        if (int.IsNegative(ID))//Checking if the ID is negative
            throw new BlWrongValueException("The task has WORNG VALUE!");
        dal.User.ResetPassword(ID, password);
    }
    private readonly IBl _bl;
    internal UserImplementation(IBl bl) => _bl = bl;
}
