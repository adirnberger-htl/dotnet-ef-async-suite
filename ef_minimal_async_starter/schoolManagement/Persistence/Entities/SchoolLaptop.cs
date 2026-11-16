namespace Persistence.Entities;

public class SchoolLaptop : BaseEntity
{
    public string SerialNr { get; set; } = string.Empty;
    public string MakeAndModel { get; set; } = string.Empty;
    public string InstalledSwImage { get; set; } = string.Empty;
    
    public Teacher Administrator { get; set; }
}