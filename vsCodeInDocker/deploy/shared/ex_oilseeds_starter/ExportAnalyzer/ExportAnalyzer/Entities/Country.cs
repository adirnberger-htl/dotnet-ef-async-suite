namespace ExportAnalyzer.Entities;

public class Country
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required Continent Continent { get; set; }

}
