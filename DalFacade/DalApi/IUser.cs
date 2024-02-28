using DO;


namespace DalApi;

public interface IUser : ICrud<User>
{
    public void ResetPassword(int ID, string password);
}
