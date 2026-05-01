namespace Habit_Tracker.Services;

public class HabitService
{
    HabitRepo HabitRepository { get; }
    HabitType? HabitType { get; set; }

    public HabitService(Database database)
    {
        HabitRepository = new HabitRepo(database);
    }

    internal void SetHabitType(HabitType type)
    {
        HabitType= type;
    }

    void DisplayHabits()
    {
        var habits = HabitRepository.GetHabitsByTypeId(HabitType!.Id);
        
        var table = new Table();
        
        table.AddColumn("[bold yellow]ID[/]");
        table.AddColumn("[bold yellow]Quantity[/]");
        table.AddColumn("[bold yellow]Date[/]");
        foreach (var habit in habits)
        {
            table.AddRow($"[red]{habit.Id}[/]", $"[red]{habit.Quantity}[/]", $"[red]{habit.Date}[/]");
        }
        
        AnsiConsole.Write(table);
    }

    internal void DisplayHabitProcess()
    {
        Console.Clear();
        Console.WriteLine("Habit Events:");

        DisplayHabits();

        Console.WriteLine("\nPress any key to return to menu.");
        Console.ReadKey();
    }

    internal void AddHabitProcess()
    {
        Console.Clear();
        Console.WriteLine($"Adding new habit for {HabitType!.Name} ({HabitType.MeasurementUnit})");

        var habitEvent = new Habit()
        {
            TypeId = HabitType.Id,
        };

        Console.WriteLine("Please enter the quantity:");
        var quantityInput = Console.ReadLine();
        while(!Int32.TryParse(quantityInput, out var quantity) || quantity == 0)
        {
            Console.WriteLine("Invalid input, please enter a non-zero number.");
            quantityInput = Console.ReadLine();
        }

        habitEvent.Quantity = Int32.Parse(quantityInput);

        Console.WriteLine("Please enter the date (YYYY-MM-DD) or leave empty for today:");
        var dateInput = Console.ReadLine();

        while(!string.IsNullOrEmpty(dateInput) && !DateTime.TryParse(dateInput, out var date))
        {
            Console.WriteLine("Invalid input, please enter a valid date (YYYY-MM-DD) or leave empty for today:");
            dateInput = Console.ReadLine();
        }

        habitEvent.Date = string.IsNullOrEmpty(dateInput) ? DateTime.Today : DateTime.Parse(dateInput);
    }

    internal void EditHabitProcess()
    {
        Console.Clear();
        Console.WriteLine("What Habit do you want to edit?");

        DisplayHabits();
        var id = Console.ReadLine();
        var habit = ValidateHabit(id);
        while (habit == null)
        {
            Console.WriteLine($"Invalid input, please enter an id of a habit.");
            id = Console.ReadLine();
            habit = ValidateHabit(id);
        }

        bool editing = true;
        while (editing)
        {
            Console.Clear();
            Console.WriteLine("What do you want to edit?");
            Console.WriteLine("1. Quantity");
            Console.WriteLine("2. Date");
            Console.WriteLine("3. Return to Main menu");

            var input = Console.ReadLine();
            while (ValidateHelper.ValidateInput(input, 1, 4) == false)
            {
                Console.WriteLine("Invalid input, please enter a number between 1 and 4.");
                input = Console.ReadLine();
            }

            switch (input)
            {
                case "1":
                    Console.WriteLine("Type a new Quantity and hit Enter.");
                    var quantityInput = Console.ReadLine();
                    while(!Int32.TryParse(quantityInput, out var quantity) || quantity == 0)
                    {
                        Console.WriteLine("Invalid input, please enter a non-zero number.");
                        quantityInput = Console.ReadLine();
                    }
                    habit.Quantity = Int32.Parse(quantityInput);
                    break;
                case "2":
                    Console.WriteLine("Type a new Date (YYYY-MM-DD) and hit Enter.");
                    var dateInput = Console.ReadLine();
                    while(!DateTime.TryParse(dateInput, out var date))
                    {
                        Console.WriteLine("Invalid input, please enter a valid date (YYYY-MM-DD).");
                        dateInput = Console.ReadLine();
                    }
                    habit.Date = DateTime.Parse(dateInput);
                    break;
                default:
                    editing = false;
                    break;
            }

            HabitRepository.UpdateHabit(habit);
        }
    }

    internal void DeleteHabitProcess()
    {
        Console.Clear();
        Console.WriteLine("What Habit type do you want to delete?");

        DisplayHabits();

        Console.WriteLine("Please enter the id of the habit you want to delete, or just hit enter to return.");
        var id = Console.ReadLine();

        if (string.IsNullOrEmpty(id))
            return;
        
        var habit = ValidateHabit(id);
        while (habit == null)
        {
            Console.WriteLine($"Invalid input, please enter an id of a habit.");
            id = Console.ReadLine();
            habit = ValidateHabit(id);
        }

        HabitRepository.DeleteHabit(habit.Id);
    }

    public Habit? ValidateHabit(string? input)
    {
        if (int.TryParse(input, out int id))
        {
            var habit = HabitRepository.GetHabitById(id);
            return habit;
        }
        return null;
    }
}