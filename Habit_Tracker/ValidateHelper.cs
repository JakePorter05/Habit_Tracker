namespace Habit_Tracker;

internal static class ValidateHelper
{
    public static bool ValidateInput(string? input, int min, int max)
    {
        if (int.TryParse(input, out int result))
        {
            if (result >= min && result <= max)
            {
                return true;
            }
        }
        return false;
    }
}