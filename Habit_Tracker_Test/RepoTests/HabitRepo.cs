namespace Habit_Tracker_Test.RepoTests;

[Collection("DatabaseRepoTests")]
public class HabitRepoTests : IDisposable
{
    private readonly string _originalCurrentDirectory;
    private readonly string _testDirectory;
    private readonly Database _database;
    private readonly HabitRepo _repo;
    private readonly HabitTypeRepo _habitTypeRepo;

    public HabitRepoTests()
    {
        _originalCurrentDirectory = AppContext.BaseDirectory;
        _testDirectory = Path.Combine(Path.GetTempPath(), $"HabitRepoTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);
        Environment.CurrentDirectory = _testDirectory;

        _database = new Database();
        _database.Initialize();

        _repo = new HabitRepo(_database);
        _habitTypeRepo = new HabitTypeRepo(_database);
    }

    [Fact]
    public void AddHabit_ShouldPersistHabit()
    {
        HabitType createdType = _habitTypeRepo.AddHabitType(new HabitType
        {
            Name = "Exercise",
            MeasurementUnit = "reps",
            Description = "Push-ups",
            AddedAt = new DateTime(2026, 1, 1)
        });

        Habit habit = new()
        {
            TypeId = createdType.Id,
            Quantity = 12,
            Date = new DateTime(2026, 1, 1)
        };

        Habit created = _repo.AddHabit(habit);
        Habit? inserted = _repo.GetHabitById(created.Id);

        Assert.True(created.Id > 0);
        Assert.NotNull(inserted);
        Assert.Equal(created.Id, inserted!.Id);
        Assert.Equal(12, inserted.Quantity);
    }

    [Fact]
    public void GetHabitById_ShouldReturnHabit_WhenHabitExists()
    {
        HabitType createdType = _habitTypeRepo.AddHabitType(new HabitType
        {
            Name = "Reading",
            MeasurementUnit = "pages",
            Description = "Read books",
            AddedAt = new DateTime(2026, 2, 1)
        });

        Habit createdHabit = _repo.AddHabit(new Habit { TypeId = createdType.Id, Quantity = 8, Date = new DateTime(2026, 2, 1) });

        Habit? habit = _repo.GetHabitById(createdHabit.Id);

        Assert.NotNull(habit);
        Assert.Equal(createdHabit.Id, habit!.Id);
        Assert.Equal(8, habit.Quantity);
    }

    [Fact]
    public void GetHabitById_ShouldReturnNull_WhenHabitDoesNotExist()
    {
        Habit? habit = _repo.GetHabitById(9999);

        Assert.Null(habit);
    }

    [Fact]
    public void UpdateHabit_ShouldUpdateExistingHabit()
    {
        HabitType createdType = _habitTypeRepo.AddHabitType(new HabitType
        {
            Name = "Water",
            MeasurementUnit = "cups",
            Description = "Drink water",
            AddedAt = new DateTime(2026, 3, 1)
        });

        Habit createdHabit = _repo.AddHabit(new Habit { TypeId = createdType.Id, Quantity = 5, Date = new DateTime(2026, 3, 1) });

        Habit updated = new()
        {
            Id = createdHabit.Id,
            TypeId = createdType.Id,
            Quantity = 20,
            Date = new DateTime(2026, 3, 2)
        };

        int result = _repo.UpdateHabit(updated);
        Habit? loaded = _repo.GetHabitById(createdHabit.Id);

        Assert.Equal(1, result);
        Assert.NotNull(loaded);
        Assert.Equal(20, loaded!.Quantity);
        Assert.Equal(new DateTime(2026, 3, 2), loaded.Date);
    }

    [Fact]
    public void DeleteHabit_ShouldRemoveHabit()
    {
        HabitType createdType = _habitTypeRepo.AddHabitType(new HabitType
        {
            Name = "Meditation",
            MeasurementUnit = "minutes",
            Description = "Meditate",
            AddedAt = new DateTime(2026, 4, 1)
        });

        Habit createdHabit = _repo.AddHabit(new Habit { TypeId = createdType.Id, Quantity = 9, Date = new DateTime(2026, 4, 1) });

        int result = _repo.DeleteHabit(createdHabit.Id);
        Habit? deleted = _repo.GetHabitById(createdHabit.Id);

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