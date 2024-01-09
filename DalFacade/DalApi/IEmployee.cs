
namespace DalApi;
using DO;
public interface IEmployee
{
    int Create(Employee item); //Creates new entity object in DAL
    Employee? Read(int id); //Reads entity object by its ID 
    List<Employee> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(Employee item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
