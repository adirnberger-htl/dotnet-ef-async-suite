using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;

namespace Services;

public class SchoolMgmtService
{
    private readonly ApplicationDbContext _context;

    public SchoolMgmtService(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task InitAsync()
    {
        ClearAsync();

        var classA = new SchoolClass { Name = "1A" };
        var classB = new SchoolClass { Name = "1B" };

        classA.Pupils.Add(new Pupil { FirstName = "Anna", LastName = "Berg", BirthDate = new DateTime(2018, 2, 12) });
        classA.Pupils.Add(new Pupil { FirstName = "Liam", LastName = "Frey", BirthDate = new DateTime(2018, 4, 23) });
        classA.Pupils.Add(new Pupil { FirstName = "Noah", LastName = "Keller", BirthDate = new DateTime(2018, 6, 5) });
        classA.Pupils.Add(new Pupil { FirstName = "Mia", LastName = "Schmid", BirthDate = new DateTime(2018, 8, 17) });
        classA.Pupils.Add(new Pupil { FirstName = "Eva", LastName = "Weiss", BirthDate = new DateTime(2018, 10, 29) });

        classB.Pupils.Add(new Pupil { FirstName = "Luca", LastName = "Ammann", BirthDate = new DateTime(2018, 1, 8) });
        classB.Pupils.Add(new Pupil { FirstName = "Lea", LastName = "Brunner", BirthDate = new DateTime(2018, 3, 19) });
        classB.Pupils.Add(new Pupil { FirstName = "Nina", LastName = "Graf", BirthDate = new DateTime(2018, 5, 31) });
        classB.Pupils.Add(new Pupil { FirstName = "Ben", LastName = "Huber", BirthDate = new DateTime(2018, 7, 14) });
        classB.Pupils.Add(new Pupil { FirstName = "Jan", LastName = "Moser", BirthDate = new DateTime(2018, 9, 25) });

        _context.SchoolClasses.AddRange(classA, classB);
        await _context.SaveChangesAsync();
    }

    public async Task ClearAsync()
    {
        try
        {
            _context.Pupils.RemoveRange(_context.Pupils);
            _context.SchoolClasses.RemoveRange(_context.SchoolClasses);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task<List<SchoolClass>> GetSchoolClassesAsync()
    {
        return await _context.SchoolClasses
            .OrderBy(schoolClass => schoolClass.Name)
            .ToListAsync();
    }

    public async Task<List<Pupil>> GetPupilsAsync(string? className = null)
    {
        IQueryable<Pupil> query = _context.Pupils.Include(pupil => pupil.SchoolClass);

        if (!string.IsNullOrWhiteSpace(className))
        {
            query = query.Where(pupil =>
                pupil.SchoolClass != null && pupil.SchoolClass.Name == className);
        }

        return await query
            .OrderBy(pupil => pupil.LastName)
            .ThenBy(pupil => pupil.FirstName)
            .ToListAsync();
    }
}
