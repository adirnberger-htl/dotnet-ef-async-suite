namespace Persistence.Entities;

public class Subject : BaseEntity
{
    public required string Title { get; set; } = string.Empty;
    
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<SchoolClass> SchoolClasses { get; set; } = new List<SchoolClass>(); 
}