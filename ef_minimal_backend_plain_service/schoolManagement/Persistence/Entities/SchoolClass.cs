namespace Persistence.Entities;

public class SchoolClass : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Pupil> Pupils { get; set; } = new List<Pupil>();

    public override string ToString() => Name;
}
