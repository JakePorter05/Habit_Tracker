namespace Habit_Tracker.Services;

internal class HabitService
{
    HabitRepo HabitRepository { get; }

    public HabitService(Database database)
    {
        HabitRepository = new HabitRepo(database);
    }

    internal void DisplayHabitProcess()
    {
        throw new NotImplementedException();
    }

    internal void AddHabitProcess()
    {
        throw new NotImplementedException();
    }

    internal void EditHabitProcess()
    {
        throw new NotImplementedException();
    }

    internal void DeleteHabitProcess()
    {
        throw new NotImplementedException();
    }
}