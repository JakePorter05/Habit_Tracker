namespace Habit_Tracker_Data.Repos;

public class HabitTypeRepo
{
    public HabitTypeRepo(Database database)
    {
        database.Initialize();
    }

    public IEnumerable<HabitType> GetAllHabitTypes()
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        List<HabitType> habitTypes = new();

        string selectQuery = "SELECT * FROM HabitType;";

        using var command = new SqliteCommand(selectQuery, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            habitTypes.Add(new HabitType
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                MeasurementUnit = reader.GetString(2),
                Description = reader.GetString(3),
                AddedAt = reader.GetDateTime(4)
            });
        }

        return habitTypes;
    }

    public HabitType? GetHabitTypeById(int id)
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        string selectQuery = "SELECT * FROM HabitType WHERE Id = @Id;";

        using var command = new SqliteCommand(selectQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new HabitType
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                MeasurementUnit = reader.GetString(2),
                Description = reader.GetString(3),
                AddedAt = reader.GetDateTime(4)
            };
        }

        return null;
    }

    public HabitType AddHabitType(HabitType habitType)
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        string insertQuery = @"
                INSERT INTO HabitType (Name, MeasurementUnit, Description, AddedAt)
                VALUES (@Name, @MeasurementUnit, @Description, @AddedAt);
            ";

        using var command = new SqliteCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@Name", habitType.Name);
        command.Parameters.AddWithValue("@MeasurementUnit", habitType.MeasurementUnit);
        command.Parameters.AddWithValue("@Description", habitType.Description);
        command.Parameters.AddWithValue("@AddedAt", habitType.AddedAt.ToString("yyyy-MM-dd"));

        command.ExecuteNonQuery();

        using var idCommand = new SqliteCommand("SELECT last_insert_rowid();", connection);
        habitType.Id = Convert.ToInt32((long)idCommand.ExecuteScalar()!);

        return habitType;
    }

    public int UpdateHabitType(HabitType habitType)
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        string updateQuery = @"
                UPDATE HabitType
                SET Name = @Name, MeasurementUnit = @MeasurementUnit, Description = @Description, AddedAt = @AddedAt
                WHERE Id = @Id;
            ";

        using var command = new SqliteCommand(updateQuery, connection);
        command.Parameters.AddWithValue("@Name", habitType.Name);
        command.Parameters.AddWithValue("@MeasurementUnit", habitType.MeasurementUnit);
        command.Parameters.AddWithValue("@Description", habitType.Description);
        command.Parameters.AddWithValue("@AddedAt", habitType.AddedAt.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@Id", habitType.Id);

        return command.ExecuteNonQuery();
    }

    public int DeleteHabitType(int id)
    {
        using var connection = new SqliteConnection(Database.ConnectionString);
        connection.Open();

        string deleteQuery = "DELETE FROM HabitType WHERE Id = @Id;";

        using var command = new SqliteCommand(deleteQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        string deleteHabitsQuery = "DELETE FROM Habit WHERE TypeId = @TypeId;";
        using var deleteHabitsCommand = new SqliteCommand(deleteHabitsQuery, connection);
        deleteHabitsCommand.Parameters.AddWithValue("@TypeId", id);
        
        var eventcount = deleteHabitsCommand.ExecuteNonQuery();
        var habit = command.ExecuteNonQuery();
        
        return eventcount + habit;
    }
}