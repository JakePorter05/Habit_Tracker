namespace Habit_Tracker_Data.Repos;

public class HabitRepo
{
    internal Database Database { get; }
    
    public HabitRepo(Database database)
    {
        Database = database;
    }

    public IEnumerable<Habit> GetAllHabits()
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        List<Habit> habits = new();

        string selectQuery = "SELECT * FROM Habit;";

        using var command = new SqliteCommand(selectQuery, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            habits.Add(new Habit
            {
                Id = reader.GetInt32(0),
                TypeId = reader.GetInt32(1),
                Quantity = reader.GetInt32(2),
                Date = reader.GetDateTime(3)
            });
        }

        return habits;
    }

    public IEnumerable<Habit> GetHabitsByTypeId(int typeId)
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        List<Habit> habits = new();
        string selectQuery = "SELECT * FROM Habit WHERE TypeId = @TypeId;";
       
        using var command = new SqliteCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@TypeId", typeId);
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            habits.Add(new Habit
            {
                Id = reader.GetInt32(0),
                TypeId = reader.GetInt32(1),
                Quantity = reader.GetInt32(2),
                Date = reader.GetDateTime(3)
            });
        }
       
        return habits;
    }

    public Habit? GetHabitById(int id)
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        string selectQuery = "SELECT * FROM Habit WHERE Id = @Id;";

        using var command = new SqliteCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Habit
            {
                Id = reader.GetInt32(0),
                TypeId = reader.GetInt32(1),
                Quantity = reader.GetInt32(2),
                Date = reader.GetDateTime(3)
            };
        }

        return null;
    }

    public Habit AddHabit(Habit habit)
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        string insertQuery = "INSERT INTO Habit (TypeId, Quantity, Date) VALUES (@TypeId, @Quantity, @Date);";

        using var command = new SqliteCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@TypeId", habit.TypeId);
        command.Parameters.AddWithValue("@Quantity", habit.Quantity);
        command.Parameters.AddWithValue("@Date", habit.Date);

        command.ExecuteNonQuery();

        using var idCommand = new SqliteCommand("SELECT last_insert_rowid();", connection);
        habit.Id = Convert.ToInt32((long)idCommand.ExecuteScalar()!);

        return habit;
    }

    public int UpdateHabit(Habit habit)
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        string updateQuery = "UPDATE Habit SET TypeId = @TypeId, Quantity = @Quantity, Date = @Date WHERE Id = @Id;";

        using var command = new SqliteCommand(updateQuery, connection);
        command.Parameters.AddWithValue("@TypeId", habit.TypeId);
        command.Parameters.AddWithValue("@Quantity", habit.Quantity);
        command.Parameters.AddWithValue("@Date", habit.Date);
        command.Parameters.AddWithValue("@Id", habit.Id);

        return command.ExecuteNonQuery();
    }

    public int DeleteHabit(int id)
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        string deleteQuery = "DELETE FROM Habit WHERE Id = @Id;";

        using var command = new SqliteCommand(deleteQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        return command.ExecuteNonQuery();
    }
}