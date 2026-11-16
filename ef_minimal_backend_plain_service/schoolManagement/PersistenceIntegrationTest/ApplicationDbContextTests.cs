using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using Persistence.Entities;
using Services;

namespace PersistenceIntegrationTest;

public class ApplicationDbContextTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly SchoolMgmtService _service;
    private readonly IDbContextTransaction _transaction;

    public ApplicationDbContextTests()
    {
        _context = new ApplicationDbContext();
        _service = new SchoolMgmtService(_context);
        _transaction = _context.Database.BeginTransaction();
    }

    [Fact]
    public void ContextAndExpectedDbSetsAreAvailable()
    {
        Assert.NotNull(_context);
        Assert.NotNull(_context.SchoolClasses);
        Assert.NotNull(_context.Pupils);

        var birthDate = _context.Model
            .FindEntityType(typeof(Pupil))!
            .FindProperty(nameof(Pupil.BirthDate))!;
        Assert.False(birthDate.IsNullable);

        Assert.False(_context.SchoolClasses.Any());
        Assert.False(_context.Pupils.Any());
    }

    [Fact]
    public async Task ServiceInitializesQueriesAndClearsSchoolData()
    {
        await _service.InitAsync();

        var classes = await _service.GetSchoolClassesAsync();
        var pupilsOfClassA = await _service.GetPupilsAsync("1A");

        Assert.Equal(new[] { "1A", "1B" }, classes.Select(schoolClass => schoolClass.Name));
        Assert.Equal(5, pupilsOfClassA.Count);
        Assert.All(pupilsOfClassA, pupil => Assert.Equal("1A", pupil.SchoolClass?.Name));

        await _service.ClearAsync();

        Assert.Empty(await _service.GetSchoolClassesAsync());
        Assert.Empty(await _service.GetPupilsAsync());
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();
        _context.Dispose();
    }
}
