namespace BlImplementation;
using BlApi;
using Bllmplementation;

internal class Bl : IBl
{
    public IEmployee Employee => new EmployeeImplementation(this);
    public ITask Task => new TaskImplementation(this);
    public IUser User =>  new UserImplementation(this);
    
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();
    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    public void AdvanceTimeByYear()
    {
        Clock = Clock.AddYears(1);
    }

    public void AdvanceTimeByMonth()
    {
        Clock = Clock.AddMonths(1);
    }

    public void AdvanceTimeByDay()
    {
        Clock = Clock.AddDays(1);
    }

    public void AdvanceTimeByHour()
    {
        Clock = Clock.AddHours(1);
    }

    public void ResetTime()
    {
        Clock = DateTime.Now.Date;
    }
}