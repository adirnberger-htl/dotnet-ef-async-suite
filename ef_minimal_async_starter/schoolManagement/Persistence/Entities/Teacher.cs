namespace Persistence.Entities;

public class Teacher : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public ICollection<SchoolLaptop> AdministeredLaptops { get; set; } = new List<SchoolLaptop>();
}