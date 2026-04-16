public class Database
{
    public const string ConnectionString = "Data Source=habits.db";
    
    internal int SeedHabitTypes(SqliteConnection connection)
    {
        //check if there are any habit types already in the database
        string checkHabitTypeQuery = "SELECT COUNT(*) FROM HabitType;";

        using var countCommand = new SqliteCommand(checkHabitTypeQuery, connection);
        var count = countCommand.ExecuteScalar();
        if (long.TryParse(count?.ToString(), out long habitTypeCount) && habitTypeCount > 0)
        {
            //there are already habit types in the database, so we can skip seeding
            return 0;
        }

        //insert some default habit types
        string insertHabitTypeQuery = @"
            INSERT INTO HabitType (Name, MeasurementUnit, Description, AddedAt) VALUES
            ('Exercise', 'reps', 'Do push-ups', '2024-01-01'),
            ('Reading', 'pages', 'Read a book', '2024-01-02'),
            ('Water', 'cups', 'Drink 8 ounces of water', '2024-01-03');
        ";

        using var insertCommand = new SqliteCommand(insertHabitTypeQuery, connection);
        insertCommand.ExecuteNonQuery();

        return 3; 
    }

    public int SeedHabits(SqliteConnection connection)
    {
        //check if there are any habits already in the database
        string checkHabitQuery = "SELECT COUNT(*) FROM Habit;";

        using var countCommand = new SqliteCommand(checkHabitQuery, connection);
        var count = countCommand.ExecuteScalar();
        if (long.TryParse(count?.ToString(), out long habitCount) && habitCount > 0)
        {
            //there are already habits in the database, so we can skip seeding
            return 0;
        }

        //insert some default habits
        string insertHabitQuery = @$"
            INSERT INTO Habit (TypeId, Quantity, Date) VALUES
            (1, 30, '2024-01-01'),
            (2, 30, '2024-01-02'),
            (3, 8, '2024-01-03');
        ";

        using var insertCommand = new SqliteCommand(insertHabitQuery, connection);
        insertCommand.ExecuteNonQuery();

        return 3;
    }

    public void Seed()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        //seed some default habit types
        int seededHabitTypes = SeedHabitTypes(connection);
        //Console.WriteLine($"Seeded {seededHabitTypes} habit types.");

        //seed some default habits
        int seededHabits = SeedHabits(connection);
        //Console.WriteLine($"Seeded {seededHabits} habits.");
    }

    public void Initialize()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        string createHabitTypeTableQuery = @"
                CREATE TABLE IF NOT EXISTS HabitType (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    MeasurementUnit TEXT,
                    Description TEXT,
                    AddedAt TEXT
                );
            ";

        using var createHabitTypeTableCommand = new SqliteCommand(createHabitTypeTableQuery, connection);
        createHabitTypeTableCommand.ExecuteNonQuery();

        string createHabitTableQuery = @"
                CREATE TABLE IF NOT EXISTS Habit (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TypeId INTEGER,
                    Quantity INTEGER,
                    Date TEXT,
                    FOREIGN KEY(TypeId) REFERENCES HabitType(Id)
                );
            ";

        using var createHabitTableCommand = new SqliteCommand(createHabitTableQuery, connection);
        createHabitTableCommand.ExecuteNonQuery();
    }
}