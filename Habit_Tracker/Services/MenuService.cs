namespace Habit_Tracker.Services;

internal class MenuService
{
    HabitService HabitService { get; }
    HabitTypeService HabitTypeService { get; }

    internal MenuService(Database database)
    {
        HabitService = new HabitService(database);
        HabitTypeService = new HabitTypeService(database);
    }

    internal void DisplayMain()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Habit Tracker!");
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. View Habits");
        Console.WriteLine("2. Add Habit");
        Console.WriteLine("3. Edit Habit");
        Console.WriteLine("4. Delete Habit");
        Console.WriteLine("5. Habit Event Menu");
        Console.WriteLine("6. Exit");

        var input = Console.ReadLine();
        while(ValidateHelper.ValidateInput(input, 1, 6) == false)
        {
            Console.WriteLine("Invalid input, please enter a number between 1 and 6.");
            input = Console.ReadLine();
        }
        
        switch (input)
        {
            case "1":
                HabitTypeService.DisplayHabitTypesProcess();
                break;
            case "2":
                HabitTypeService.AddHabitTypeProcess();
                break;
            case "3":
                HabitTypeService.EditHabitTypeProcess();
                break;
            case "4":
                HabitTypeService.DeleteHabitTypeProcess();
                break;
            case "5":
                var type = HabitTypeService.SelectHabitTypeProcess();
                DisplayHabitEventMenu(type);
                break;
            default:
                Environment.Exit(0);
                break;
        }
    }

    internal void DisplayHabitEventMenu(HabitType type)
    {
        var process = true;
        while (process)
        {
            Console.Clear();
            Console.WriteLine($"{type.Name}:{type.MeasurementUnit}");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. View Events for Habit");
            Console.WriteLine("2. Add Event");
            Console.WriteLine("3. Edit Event");
            Console.WriteLine("4. Delete Event");
            Console.WriteLine("5. Return to Main Menu");

            var input = Console.ReadLine();
            while (ValidateHelper.ValidateInput(input, 1, 6) == false)
            {
                Console.WriteLine("Invalid input, please enter a number between 1 and 6.");
                input = Console.ReadLine();
            }

            switch (input)
            {
                case "1":
                    HabitService.DisplayHabitProcess();
                    break;
                case "2":
                    HabitService.AddHabitProcess();
                    break;
                case "3":
                    HabitService.EditHabitProcess();
                    break;
                case "4":
                    HabitService.DeleteHabitProcess();
                    break;
                default:
                    process = false;
                    break;
            }
        }
    }
}