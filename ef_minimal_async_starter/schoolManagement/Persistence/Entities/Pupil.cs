namespace Persistence.Entities;

public class Pupil : BaseEntity
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public required DateTime BirthDate { get; set; }
    
    public int SchoolClassId { get; set; }
    public SchoolClass SchoolClass { get; set; } = null!;
    
    public SchoolLaptop? SchoolLaptop { get; set; }

    public override string ToString() => $"{LastName}, {FirstName}";
}
