namespace Habit_Tracker_Test.RepoTests;

public class ValidationTests
{
    [Fact]
    public void ValidateHabitType_ShouldReturnNull_WhenInputIsNotAnInteger()
    {
        var service = new HabitTypeService(new Database());
        var result = service.ValidateHabitType("abc");
        Assert.Null(result);
    }

    [Fact]
    public void ValidateHabitType_ShouldReturnNull_WhenHabitTypeDoesNotExist()
    {
        var service = new HabitTypeService(new Database());
        var result = service.ValidateHabitType("9999");
        Assert.Null(result);
    }

    [Fact]
    public void ValidateHabitType_ShouldReturnHabitType_WhenInputIsValid()
    {
        var database = new Database();
        database.Initialize();

        var habitTypeRepo = new HabitTypeRepo(database);
        var createdType = habitTypeRepo.AddHabitType(new HabitType
        {
            Name = "Test",
            MeasurementUnit = "units",
            Description = "Test description",
            AddedAt = DateTime.Now
        });

        var service = new HabitTypeService(database);
        var result = service.ValidateHabitType(createdType.Id.ToString());
        
        Assert.NotNull(result);
        Assert.Equal(createdType.Id, result!.Id);
    }

    [Fact]
    public void ValidateHabit_ShouldReturnFalse_WhenInputIsNotAnInteger()
    {
        var service = new HabitService(new Database());
        var result = service.ValidateHabit("abc");
        Assert.Null(result);
    }

    [Fact]
    public void ValidateHabit_ShouldReturnFalse_WhenHabitDoesNotExist()
    {
        var service = new HabitService(new Database());
        var result = service.ValidateHabit("9999");
        Assert.Null(result);
    }

    [Fact]
    public void ValidateHabit_ShouldReturnTrue_WhenInputIsValid()
    {
        var database = new Database();
        database.Initialize();

        var habitTypeRepo = new HabitTypeRepo(database);
        var createdType = habitTypeRepo.AddHabitType(new HabitType
        {
            Name = "Test",
            MeasurementUnit = "units",
            Description = "Test description",
            AddedAt = DateTime.Now
        });
        var habitRepo = new HabitRepo(database);
        var createdHabit = habitRepo.AddHabit(new Habit { TypeId = createdType.Id, Quantity = 5, Date = DateTime.Now });
        
        var service = new HabitService(database);
        var result = service.ValidateHabit(createdHabit.Id.ToString());
        
        Assert.NotNull(result);
        Assert.Equal(createdHabit.Id, result!.Id);
    }
}