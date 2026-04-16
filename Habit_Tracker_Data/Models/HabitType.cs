namespace Habit_Tracker_Data.Models;

public class HabitType
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? MeasurementUnit { get; set; }
    public string? Description { get; set; }
    public DateTime AddedAt { get; set; }
}