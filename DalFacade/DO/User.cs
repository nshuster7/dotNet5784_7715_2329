
namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="ID">Unique ID of User</param>
/// <param name="Name">Unique name of User</param>
/// <param name="Password">Unique password of User</param>
/// <param name="IsManeger">Is the user a manager</param>
public record User
(
    int ID,
    string Name,
    string Password,
    bool IsManeger
)
{
    public User() : this(0," "," ",false) { } // Empty constructor
}
