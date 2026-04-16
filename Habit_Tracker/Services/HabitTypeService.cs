namespace Habit_Tracker.Services;

internal class HabitTypeService
{
    HabitTypeRepo HabitTypeRepository { get; }

    public HabitTypeService(Database database)
    {
        HabitTypeRepository = new HabitTypeRepo(database);
    }

    int DisplayHabitTypes()
    {
        var habitTypes = HabitTypeRepository.GetAllHabitTypes();

        var table = new Table();
        table.AddColumn("[bold yellow]ID[/]");
        table.AddColumn("[bold yellow]Name[/]");
        table.AddColumn("[bold yellow]Measurement Unit[/]");
        table.AddColumn("[bold yellow]Description[/]");
        table.AddColumn("[bold yellow]Added At[/]");

        foreach (var habit in habitTypes)
        {
            table.AddRow($"[red]{habit.Id}[/]", $"[red]{habit.Name}[/]", $"[red]{habit.MeasurementUnit}[/]", $"[red]{habit.Description}[/]", $"[red]{habit.AddedAt}[/]");
        }

        AnsiConsole.Write(table);

        return habitTypes.Count();
    }

    internal HabitType SelectHabitTypeProcess()
    {
        Console.Clear();
        Console.WriteLine("What Habit type do you want to interact with?");

        var count = DisplayHabitTypes();
        var id = Console.ReadLine();
        var habitType = ValidateHabitType(id);
        while (habitType == null)
        {
            Console.WriteLine($"Invalid input, please enter an id of a habit.");
            id = Console.ReadLine();
            habitType = ValidateHabitType(id);
        }
        return habitType;
    }

    internal void DisplayHabitTypesProcess()
    {
        Console.Clear();
        Console.WriteLine("Your Habits:");

        DisplayHabitTypes();

        Console.WriteLine("\nPress any key to return to menu.");
        Console.ReadKey();
    }

    internal void AddHabitTypeProcess()
    {
        Console.Clear();

        //Name
        Console.WriteLine("Type the Name of the Habit and hit Enter.");
        var name = Console.ReadLine();

        //MeasureUnit
        Console.WriteLine("What is the unit to measure the Habit? Ex. Number of glass.");
        Console.WriteLine("Type the name of the unit and hit Enter.");
        var unit = Console.ReadLine();

        //Description
        Console.WriteLine("Type a Description of the habit and Hit Enter.");
        var description = Console.ReadLine();

        var habitType = new HabitType
        {
            AddedAt = DateTime.Now,
            Description = description,
            MeasurementUnit = unit,
            Name = name
        };
        var newType = HabitTypeRepository.AddHabitType(habitType);
        Console.WriteLine($"The id of the new Habit Type {newType.Id}");
        Console.WriteLine("\nPress any key to return to the menu.");
        Console.ReadKey();
    }

    internal void EditHabitTypeProcess()
    {
        Console.Clear();
        Console.WriteLine("What Habit type do you want to edit?");

        var count = DisplayHabitTypes();
        var id = Console.ReadLine();
        var habitType = ValidateHabitType(id);
        while (habitType == null)
        {
            Console.WriteLine($"Invalid input, please enter an id of a habit.");
            id = Console.ReadLine();
            habitType = ValidateHabitType(id);
        }

        bool editing = true;
        while (editing)
        {
            Console.Clear();
            Console.WriteLine("What do you want to edit?");
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Measurement Unit");
            Console.WriteLine("3. Description");
            Console.WriteLine("4. Return to Main menu");

            var input = Console.ReadLine();
            while (ValidateHelper.ValidateInput(input, 1, 4) == false)
            {
                Console.WriteLine("Invalid input, please enter a number between 1 and 4.");
                input = Console.ReadLine();
            }

            switch (input)
            {
                case "1":
                    Console.WriteLine("Type a new Name and hit Enter.");
                    var name = Console.ReadLine();
                    habitType.Name = name; 
                    break;
                case "2":
                    Console.WriteLine("Type a new Measurement Unit and hit Enter.");
                    var unit = Console.ReadLine();
                    habitType.MeasurementUnit = unit;
                    break;
                case "3":
                    Console.WriteLine("Type a new Description and hit Enter.");
                    var description = Console.ReadLine();
                    habitType.Name = description;
                    break;
                default:
                    editing = false;
                    break;
            }

            HabitTypeRepository.UpdateHabitType(habitType);
        }

    }

    internal void DeleteHabitTypeProcess()
    {
        Console.Clear();
        Console.WriteLine("What Habit type do you want to delete?");

        var count = DisplayHabitTypes();
        var id = Console.ReadLine();
        var habitType = ValidateHabitType(id);
        while (habitType == null)
        {
            Console.WriteLine($"Invalid input, please enter an id of a habit.");
            id = Console.ReadLine();
            habitType = ValidateHabitType(id);
        }

        HabitTypeRepository.DeleteHabitType(habitType.Id);
    }

    HabitType? ValidateHabitType(string? input)
    {
        if (int.TryParse(input, out int id))
        {
            var habitType = HabitTypeRepository.GetHabitTypeById(id);
            return habitType;
        }
        return null;
    }
}
