namespace Persistence.Entities;

public class SchoolClass : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Pupil> Pupils { get; set; } = new List<Pupil>();
    public ICollection<Subject>  Subjects { get; set; } = new List<Subject>();

    public override string ToString() => Name;
}
