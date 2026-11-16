using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using Persistence.Entities;
using Services;

namespace PersistenceIntegrationTest;
    //TODO 3 Comment in tests to check the model
    /*

public class ApplicationDbContextTests : IAsyncLifetime
{
    private ApplicationDbContext _context = null!;
    private SchoolMgmtService _service = null!;
    private IDbContextTransaction _transaction = null!;
    
    public async Task InitializeAsync()
    {
        _context = new ApplicationDbContext();
        _service = new SchoolMgmtService(_context);
        _transaction = await _context.Database.BeginTransactionAsync();
        await ClearDatabaseAsync();
    }


    [Fact]
    public async Task ContextAndExpectedDbSetsAreAvailable()
    {
        Assert.NotNull(_context.SchoolClasses);
        Assert.NotNull(_context.Pupils);
        Assert.NotNull(_context.SchoolLaptops);
        Assert.NotNull(_context.Subjects);
        Assert.NotNull(_context.Teachers);

        var birthDate = _context.Model
            .FindEntityType(typeof(Pupil))!
            .FindProperty(nameof(Pupil.BirthDate))!;
        Assert.False(birthDate.IsNullable);

        var laptopEntity = _context.Model.FindEntityType(typeof(SchoolLaptop))!;
        Assert.True(laptopEntity.FindProperty(nameof(SchoolLaptop.PupilId))!.IsNullable);
        Assert.False(laptopEntity.FindProperty(nameof(SchoolLaptop.AdministratorId))!.IsNullable);

        Assert.False(await _context.SchoolClasses.AnyAsync());
        Assert.False(await _context.Pupils.AnyAsync());
        Assert.False(await _context.SchoolLaptops.AnyAsync());
        Assert.False(await _context.Subjects.AnyAsync());
        Assert.False(await _context.Teachers.AnyAsync());
    }

    [Fact]
    public void ModelContainsExpectedConstraints()
    {
        var pupil = _context.Model.FindEntityType(typeof(Pupil))!;
        Assert.Equal(50, pupil.FindProperty(nameof(Pupil.FirstName))!.GetMaxLength());
        Assert.Equal(50, pupil.FindProperty(nameof(Pupil.LastName))!.GetMaxLength());
        Assert.Contains(
            pupil.GetIndexes(),
            index => index.IsUnique
                && index.Properties.Select(property => property.Name).SequenceEqual(
                    [nameof(Pupil.FirstName), nameof(Pupil.LastName), nameof(Pupil.BirthDate)]));

        var teacher = _context.Model.FindEntityType(typeof(Teacher))!;
        Assert.Equal(50, teacher.FindProperty(nameof(Teacher.FirstName))!.GetMaxLength());
        Assert.Equal(50, teacher.FindProperty(nameof(Teacher.LastName))!.GetMaxLength());

        var schoolClass = _context.Model.FindEntityType(typeof(SchoolClass))!;
        Assert.Equal(100, schoolClass.FindProperty(nameof(SchoolClass.Name))!.GetMaxLength());
        AssertUniqueIndex(schoolClass, nameof(SchoolClass.Name));

        var subject = _context.Model.FindEntityType(typeof(Subject))!;
        Assert.Equal(100, subject.FindProperty(nameof(Subject.Title))!.GetMaxLength());
        Assert.False(subject.FindProperty(nameof(Subject.Title))!.IsNullable);
        AssertUniqueIndex(subject, nameof(Subject.Title));

        var laptop = _context.Model.FindEntityType(typeof(SchoolLaptop))!;
        Assert.Equal(100, laptop.FindProperty(nameof(SchoolLaptop.SerialNr))!.GetMaxLength());
        Assert.False(laptop.FindProperty(nameof(SchoolLaptop.SerialNr))!.IsNullable);
        Assert.False(laptop.FindProperty(nameof(SchoolLaptop.MakeAndModel))!.IsNullable);
        Assert.False(laptop.FindProperty(nameof(SchoolLaptop.InstalledSWImage))!.IsNullable);
        Assert.False(laptop.FindProperty(nameof(SchoolLaptop.AdministratorId))!.IsNullable);
        AssertUniqueIndex(laptop, nameof(SchoolLaptop.SerialNr));
        AssertUniqueIndex(laptop, nameof(SchoolLaptop.PupilId));
    }

    [Fact]
    public async Task DuplicatePupilIdentityIsRejected()
    {
        var schoolClass = new SchoolClass { Name = "1A" };
        var birthDate = UtcToday.AddYears(-12);
        schoolClass.Pupils.Add(new Pupil
        {
            FirstName = "Anna",
            LastName = "Berg",
            BirthDate = birthDate
        });
        schoolClass.Pupils.Add(new Pupil
        {
            FirstName = "Anna",
            LastName = "Berg",
            BirthDate = birthDate
        });

        _context.SchoolClasses.Add(schoolClass);

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task DuplicateSchoolClassNameIsRejected()
    {
        _context.SchoolClasses.AddRange(
            new SchoolClass { Name = "1A" },
            new SchoolClass { Name = "1A" });

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task DuplicateSubjectTitleIsRejected()
    {
        _context.Subjects.AddRange(
            new Subject { Title = "Mathematics" },
            new Subject { Title = "Mathematics" });

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task DuplicateLaptopSerialNumberIsRejected()
    {
        var administrator = new Teacher { FirstName = "Maria", LastName = "Mayer" };
        administrator.AdministeredLaptops.Add(new SchoolLaptop
        {
            SerialNr = "LT-1001",
            MakeAndModel = "Lenovo ThinkPad",
            InstalledSWImage = "School-2026"
        });
        administrator.AdministeredLaptops.Add(new SchoolLaptop
        {
            SerialNr = "LT-1001",
            MakeAndModel = "Dell Latitude",
            InstalledSWImage = "School-2026"
        });

        _context.Teachers.Add(administrator);

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task IdentifierLongerThanMaximumIsRejected()
    {
        _context.SchoolClasses.Add(new SchoolClass { Name = new string('X', 101) });

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Theory]
    [InlineData(nameof(SchoolLaptop.SerialNr))]
    [InlineData(nameof(SchoolLaptop.MakeAndModel))]
    [InlineData(nameof(SchoolLaptop.InstalledSWImage))]
    public async Task LaptopWithoutRequiredStringIsRejected(string propertyName)
    {
        var administrator = new Teacher { FirstName = "Maria", LastName = "Mayer" };
        var laptop = new SchoolLaptop
        {
            SerialNr = "LT-1001",
            MakeAndModel = "Lenovo ThinkPad",
            InstalledSWImage = "School-2026",
            Administrator = administrator
        };

        switch (propertyName)
        {
            case nameof(SchoolLaptop.SerialNr):
                laptop.SerialNr = null!;
                break;
            case nameof(SchoolLaptop.MakeAndModel):
                laptop.MakeAndModel = null!;
                break;
            case nameof(SchoolLaptop.InstalledSWImage):
                laptop.InstalledSWImage = null!;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        _context.SchoolLaptops.Add(laptop);

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task LaptopWithoutAdministratorIsRejected()
    {
        _context.SchoolLaptops.Add(new SchoolLaptop
        {
            SerialNr = "LT-1001",
            MakeAndModel = "Lenovo ThinkPad",
            InstalledSWImage = "School-2026"
        });

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task TwoLaptopsForOnePupilAreRejected()
    {
        var administrator = new Teacher { FirstName = "Maria", LastName = "Mayer" };
        var schoolClass = new SchoolClass { Name = "1A" };
        var pupil = new Pupil
        {
            FirstName = "Anna",
            LastName = "Berg",
            BirthDate = UtcToday.AddYears(-12),
            SchoolClass = schoolClass
        };

        _context.AddRange(administrator, pupil);
        await _context.SaveChangesAsync();

        var administratorId = administrator.Id;
        var pupilId = pupil.Id;
        _context.ChangeTracker.Clear();

        _context.SchoolLaptops.AddRange(
            new SchoolLaptop
            {
                SerialNr = "LT-1001",
                MakeAndModel = "Lenovo ThinkPad",
                InstalledSWImage = "School-2026",
                AdministratorId = administratorId,
                PupilId = pupilId
            },
            new SchoolLaptop
            {
                SerialNr = "LT-1002",
                MakeAndModel = "Dell Latitude",
                InstalledSWImage = "School-2026",
                AdministratorId = administratorId,
                PupilId = pupilId
            });

        await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task GetPupilsWithLaptopFiltersByClass()
    {
        var (classA, classB) = CreateClassesWithPupils();
        await _context.SchoolClasses.AddRangeAsync(classA, classB);
        await _context.SaveChangesAsync();

        var pupils = await _service.GetPupilsWithLaptop("1A");

        var pupil = Assert.Single(pupils);
        Assert.Equal("Anna", pupil.FirstName);
        Assert.Equal("1A", pupil.SchoolClass.Name);
        Assert.NotNull(pupil.SchoolLaptop);
    }

    [Fact]
    public async Task GetPupilsOfClassOlderThanAsyncFiltersByAge()
    {
        var (classA, classB) = CreateClassesWithPupils();
        await _context.SchoolClasses.AddRangeAsync(classA, classB);
        await _context.SaveChangesAsync();

        var pupils = await _service.GetPupilsOfClassOlderThanAsync("1A", 12);

        Assert.Equal(["Anna"], pupils.Select(pupil => pupil.FirstName));
    }

    [Fact]
    public async Task GetTeachersForClassAsyncGroupsTeachersBySubject()
    {
        var classA = new SchoolClass { Name = "1A" };
        var classB = new SchoolClass { Name = "1B" };
        var mathematics = new Subject { Title = "Mathematics" };
        var english = new Subject { Title = "English" };

        mathematics.Teachers.Add(new Teacher { FirstName = "Maria", LastName = "Mayer" });
        mathematics.Teachers.Add(new Teacher { FirstName = "Tim", LastName = "Bauer" });
        mathematics.SchoolClasses.Add(classA);
        english.Teachers.Add(new Teacher { FirstName = "Eva", LastName = "Klein" });
        english.SchoolClasses.Add(classB);

        await _context.Subjects.AddRangeAsync(mathematics, english);
        await _context.SaveChangesAsync();

        var results = (await _service.GetTeachersForClassAsync("1A")).ToArray();

        var result = Assert.Single(results);
        Assert.Equal("Mathematics", result.Subject.Title);
        Assert.Equal(
            ["Bauer", "Mayer"],
            result.Teachers.Select(teacher => teacher.LastName));
    }

    [Fact]
    public async Task GetLaptopsOfAdminReturnsOnlyTheAdministratorsLaptops()
    {
        var admin = new Teacher { FirstName = "Maria", LastName = "Mayer" };
        var otherAdmin = new Teacher { FirstName = "Thomas", LastName = "Huber" };

        admin.AdministeredLaptops.Add(new SchoolLaptop
        {
            SerialNr = "LT-2002",
            MakeAndModel = "Dell Latitude",
            InstalledSWImage = "School-2026"
        });
        admin.AdministeredLaptops.Add(new SchoolLaptop
        {
            SerialNr = "LT-2001",
            MakeAndModel = "Lenovo ThinkPad",
            InstalledSWImage = "School-2026"
        });
        otherAdmin.AdministeredLaptops.Add(new SchoolLaptop
        {
            SerialNr = "LT-3001",
            MakeAndModel = "HP ProBook",
            InstalledSWImage = "School-2026"
        });

        await _context.Teachers.AddRangeAsync(admin, otherAdmin);
        await _context.SaveChangesAsync();
        var adminId = admin.Id;
        _context.ChangeTracker.Clear();

        var laptops = (await _service.GetLaptopsOfAdmin(adminId)).ToArray();

        Assert.Equal(["LT-2001", "LT-2002"], laptops.Select(laptop => laptop.SerialNr));
        Assert.All(laptops, laptop => Assert.Equal(adminId, laptop.Administrator.Id));
        Assert.All(laptops, laptop => Assert.Null(laptop.Pupil));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetPupilsWithLaptopWithoutClassFilterReturnsAllPupilsWithLaptop(string? schoolClassName)
    {
        var (classA, classB) = CreateClassesWithPupils();
        await _context.SchoolClasses.AddRangeAsync(classA, classB);
        await _context.SaveChangesAsync();

        var pupils = await _service.GetPupilsWithLaptop(schoolClassName);

        Assert.Equal(2, pupils.Length);
        Assert.Contains(pupils, p => p.FirstName == "Anna" && p.SchoolClass.Name == "1A");
        Assert.Contains(pupils, p => p.FirstName == "Luca" && p.SchoolClass.Name == "1B");
        Assert.All(pupils, p => Assert.NotNull(p.SchoolLaptop));
    }

    [Fact]
    public async Task GetPupilsWithLaptopReturnsEmptyWhenClassDoesNotExist()
    {
        var (classA, classB) = CreateClassesWithPupils();
        await _context.SchoolClasses.AddRangeAsync(classA, classB);
        await _context.SaveChangesAsync();

        var pupils = await _service.GetPupilsWithLaptop("NonExistentClass");

        Assert.Empty(pupils);
    }

    [Fact]
    public async Task GetPupilsOfClassOlderThanAsyncReturnsEmptyWhenNoPupilsMatch()
    {
        var (classA, classB) = CreateClassesWithPupils();
        await _context.SchoolClasses.AddRangeAsync(classA, classB);
        await _context.SaveChangesAsync();

        var pupilsInClassTooYoung = await _service.GetPupilsOfClassOlderThanAsync("1A", 20);
        var pupilsInUnknownClass = await _service.GetPupilsOfClassOlderThanAsync("NonExistentClass", 10);

        Assert.Empty(pupilsInClassTooYoung);
        Assert.Empty(pupilsInUnknownClass);
    }

    [Fact]
    public async Task GetTeachersForClassAsyncReturnsEmptyForClassWithoutSubjectsOrUnknownClass()
    {
        var classWithoutSubjects = new SchoolClass { Name = "1C" };
        await _context.SchoolClasses.AddAsync(classWithoutSubjects);
        await _context.SaveChangesAsync();

        var resultsEmptyClass = await _service.GetTeachersForClassAsync("1C");
        var resultsUnknownClass = await _service.GetTeachersForClassAsync("NonExistentClass");

        Assert.Empty(resultsEmptyClass);
        Assert.Empty(resultsUnknownClass);
    }

    [Fact]
    public async Task GetLaptopsOfAdminReturnsEmptyWhenAdminHasNoLaptopsOrDoesNotExist()
    {
        var adminWithoutLaptops = new Teacher { FirstName = "Sarah", LastName = "Connor" };
        await _context.Teachers.AddAsync(adminWithoutLaptops);
        await _context.SaveChangesAsync();

        var emptyForTeacher = await _service.GetLaptopsOfAdmin(adminWithoutLaptops.Id);
        var emptyForInvalidId = await _service.GetLaptopsOfAdmin(-999);

        Assert.Empty(emptyForTeacher);
        Assert.Empty(emptyForInvalidId);
    }

    public async Task DisposeAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        await _context.DisposeAsync();
    }

    private static (SchoolClass ClassA, SchoolClass ClassB) CreateClassesWithPupils()
    {
        var classA = new SchoolClass { Name = "1A" };
        var classB = new SchoolClass { Name = "1B" };
        var administrator = new Teacher { FirstName = "Alex", LastName = "Admin" };

        classA.Pupils.Add(new Pupil
        {
            FirstName = "Anna",
            LastName = "Berg",
            BirthDate = UtcToday.AddYears(-13).AddDays(-1),
            SchoolLaptop = new SchoolLaptop
            {
                SerialNr = "LT-TEST-1",
                MakeAndModel = "Test Laptop",
                InstalledSWImage = "Test Image",
                Administrator = administrator
            }
        });
        classA.Pupils.Add(new Pupil
        {
            FirstName = "Liam",
            LastName = "Frey",
            BirthDate = UtcToday.AddYears(-11)
        });
        classA.Pupils.Add(new Pupil
        {
            FirstName = "Noah",
            LastName = "Keller",
            BirthDate = UtcToday.AddYears(-10)
        });

        classB.Pupils.Add(new Pupil
        {
            FirstName = "Luca",
            LastName = "Ammann",
            BirthDate = UtcToday.AddYears(-13),
            SchoolLaptop = new SchoolLaptop
            {
                SerialNr = "LT-TEST-2",
                MakeAndModel = "Test Laptop",
                InstalledSWImage = "Test Image",
                Administrator = administrator
            }
        });

        return (classA, classB);
    }

    private static DateTime UtcToday => DateTime.UtcNow.Date;

    private static void AssertUniqueIndex(
        Microsoft.EntityFrameworkCore.Metadata.IEntityType entityType,
        string propertyName)
    {
        Assert.Contains(
            entityType.GetIndexes(),
            index => index.IsUnique
                && index.Properties.Count == 1
                && index.Properties[0].Name == propertyName);
    }

    private async Task ClearDatabaseAsync()
    {
        await _context.SchoolLaptops.ExecuteDeleteAsync();
        await _context.Pupils.ExecuteDeleteAsync();
        await _context.SchoolClasses.ExecuteDeleteAsync();
        await _context.Subjects.ExecuteDeleteAsync();
        await _context.Teachers.ExecuteDeleteAsync();
    }
}
*/
