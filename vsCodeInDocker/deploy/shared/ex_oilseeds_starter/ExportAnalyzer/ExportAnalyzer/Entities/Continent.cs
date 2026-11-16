namespace ExportAnalyzer.Entities;

public class Continent
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string Hemisphere { get; set; }

    public long Population { get; set; }

    public ICollection<Country> Countries { get; } = new List<Country>();
    public ICollection<ExportData> Exports { get; } = new List<ExportData>();
}