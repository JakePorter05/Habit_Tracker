namespace Habit_Tracker_Data.Models;

public class Habit
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
}