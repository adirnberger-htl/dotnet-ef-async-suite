using ExportAnalyzer.Entities;
using System.Globalization;

namespace ExportAnalyzer;

public partial class ApplicationDataContext
{
    private record ExportDataCsv(string ContinentId, string Continent, string CountryId, string Country, string ISO3, double TradeValue);

    public ISet<ExportData> ExportDataSet { get; private set; }
    public ISet<Continent> ContinentDataSet { get; private set; }
    public ISet<Country> CountryDataSet { get; private set; }

    public ApplicationDataContext()
    {
        ExportDataSet = new HashSet<ExportData>();
        ContinentDataSet = new HashSet<Continent>();
        CountryDataSet = new HashSet<Country>();
        AddContinentData();
    }

    public void LoadExportData(string fileName, Commodity commodity)
    {
        // EU,Europe,FR,France,FRA,1234.56
        // 1. Parse CSV into records
        var lines = File.ReadLines(fileName)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        if (lines.Count <= 1) return;

        var csvRecords = lines
            .Skip(1)
            .Select(line => line.Split(","))
            .Select(parts => new ExportDataCsv(
                parts[0].Trim(),
                parts[1].Trim(),
                parts[2].Trim(),
                parts[3].Trim(),
                parts[4].Trim(),
                double.Parse(parts[5].Trim().Replace(',', '.'), CultureInfo.InvariantCulture)
            ))
            .ToList();

        // 2. Convert ContinentDataSet into local dictionary
        var continentDict = ContinentDataSet.ToDictionary(c => c.Id, StringComparer.OrdinalIgnoreCase);

        // 3. Extract countries from CSV into local dictionary
        var countryDict = csvRecords
            .GroupBy(r => r.CountryId)
            .Select(g => g.First())
            .Select(r => new Country
            {
                Id = r.CountryId,
                Name = r.Country,
                Continent = continentDict.TryGetValue(r.ContinentId, out var continent) ? continent : null!
            })
            .ToDictionary(c => c.Id, StringComparer.OrdinalIgnoreCase);

        // Update each Continent with the corresponding Countries using GroupBy and foreach
        var countriesByContinent = countryDict.Values
            .Where(c => c.Continent != null)
            .GroupBy(c => c.Continent);

        foreach (var group in countriesByContinent)
        {
            foreach (var country in group)
            {
                group.Key.Countries.Add(country);
            }
        }

        // 4. Extract export data referencing objects from the local dictionaries
        var exportData = csvRecords
            .Select(r => new ExportData
            {
                Country = countryDict[r.CountryId],
                Commodity = commodity,
                TradeValue = r.TradeValue
            })
            .ToList();

        // 5. Put objects in corresponding hash set properties
        CountryDataSet.UnionWith(countryDict.Values);
        ExportDataSet.UnionWith(exportData);
    }

    private void AddContinentData()
    {
            //from https://www.worldometers.info/geography/7-continents/
            //Suppose we have another data table like this:
            ContinentDataSet = new HashSet<Continent> {
                new Continent {Id = "as", Name = "Asia", Hemisphere = "East", Population = 3261050390},
                new Continent {Id = "af", Name = "Africa", Hemisphere = "South", Population = 1340598147},
                new Continent {Id = "eu", Name = "Europa", Hemisphere = "North", Population = 747636026},
                new Continent {Id = "na", Name = "North America", Hemisphere = "North", Population = 592072212},
                new Continent {Id = "sa", Name = "South America", Hemisphere = "South", Population = 430759766},
                new Continent {Id = "oc", Name = "Oceania", Hemisphere = "East", Population = 43111704},
                new Continent {Id = "an", Name = "Antarctica", Hemisphere = "South", Population = 0},
            };
    }
    
}