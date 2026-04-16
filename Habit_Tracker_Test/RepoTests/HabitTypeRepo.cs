namespace Habit_Tracker_Test.RepoTests;

[Collection("DatabaseRepoTests")]
public class HabitTypeRepoTests : IDisposable
{
    private readonly string _originalCurrentDirectory;
    private readonly string _testDirectory;
    private readonly Database _database;
    private readonly HabitTypeRepo _repo;

    public HabitTypeRepoTests()
    {
        _originalCurrentDirectory = AppContext.BaseDirectory;
        _testDirectory = Path.Combine(Path.GetTempPath(), $"HabitTypeRepoTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);
        Environment.CurrentDirectory = _testDirectory;

        _database = new Database();
        _database.Initialize();

        _repo = new HabitTypeRepo(_database);
    }

    [Fact]
    public void AddHabitType_ShouldPersistHabitType()
    {
        HabitType habitType = new()
        {
            Name = "Exercise",
            MeasurementUnit = "reps",
            Description = "Push-ups",
            AddedAt = new DateTime(2026, 1, 1)
        };

        HabitType created = _repo.AddHabitType(habitType);
        HabitType? inserted = _repo.GetHabitTypeById(created.Id);

        Assert.True(created.Id > 0);
        Assert.NotNull(inserted);
        Assert.Equal(created.Id, inserted!.Id);
        Assert.Equal("Push-ups", inserted.Description);
    }

    [Fact]
    public void GetHabitTypeById_ShouldReturnHabitType_WhenHabitTypeExists()
    {
        HabitType created = _repo.AddHabitType(new HabitType
        {
            Name = "Reading",
            MeasurementUnit = "pages",
            Description = "Read books",
            AddedAt = new DateTime(2026, 2, 1)
        });

        HabitType? habitType = _repo.GetHabitTypeById(created.Id);

        Assert.NotNull(habitType);
        Assert.Equal(created.Id, habitType!.Id);
        Assert.Equal("Reading", habitType.Name);
    }

    [Fact]
    public void GetHabitTypeById_ShouldReturnNull_WhenHabitTypeDoesNotExist()
    {
        HabitType? habitType = _repo.GetHabitTypeById(9999);

        Assert.Null(habitType);
    }

    [Fact]
    public void UpdateHabitType_ShouldUpdateExistingHabitType()
    {
        HabitType created = _repo.AddHabitType(new HabitType
        {
            Name = "Water",
            MeasurementUnit = "cups",
            Description = "Drink water",
            AddedAt = new DateTime(2026, 3, 1)
        });

        HabitType updated = new()
        {
            Id = created.Id,
            Name = "Hydration",
            MeasurementUnit = "glasses",
            Description = "Drink more water",
            AddedAt = new DateTime(2026, 3, 2)
        };

        int result = _repo.UpdateHabitType(updated);
        HabitType? loaded = _repo.GetHabitTypeById(created.Id);

        Assert.Equal(1, result);
        Assert.NotNull(loaded);
        Assert.Equal("Hydration", loaded!.Name);
        Assert.Equal("glasses", loaded.MeasurementUnit);
    }

    [Fact]
    public void DeleteHabitType_ShouldRemoveHabitType()
    {
        HabitType created = _repo.AddHabitType(new HabitType
        {
            Name = "Meditation",
            MeasurementUnit = "minutes",
            Description = "Meditate",
            AddedAt = new DateTime(2026, 4, 1)
        });

        int result = _repo.DeleteHabitType(created.Id);
        HabitType? deleted = _repo.GetHabitTypeById(created.Id);

        Assert.Equal(1, result);
        Assert.Null(deleted);
    }

    public void Dispose()
    {
        Environment.CurrentDirectory = _originalCurrentDirectory;

        if (!Directory.Exists(_testDirectory))
        {
            return;
        }

        try
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}