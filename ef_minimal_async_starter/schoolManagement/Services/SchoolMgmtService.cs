using Persistence;
using Persistence.DTO;
using Persistence.Entities;

namespace Services;

public class SchoolMgmtService(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    //TODO 6 Using AI, create test cases for these methods.
    //TODO 7 Implement these queries (yourself!!)

    public Task<Pupil[]> GetPupilsWithLaptop(string? schoolClassName)
    {
        throw new NotImplementedException();
    }

    public Task<Pupil[]> GetPupilsOfClassOlderThanAsync(string schoolClassName, int minAge)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TeachersForClassResult>> GetTeachersForClassAsync(string schoolClassName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SchoolLaptop>> GetLaptopsOfAdmin(int teacherId)
    {
        throw new NotImplementedException();
    }

}
