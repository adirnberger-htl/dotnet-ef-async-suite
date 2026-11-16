using System.Linq;
using ExportAnalyzer;
using ExportAnalyzer.Entities;
using Xunit;

namespace ExportAnalyzerTest;

public class ApplicationDataContextTest
{
    private const string PathToSmallData = "SmallExportTestData.csv";
    private const string PathToSmallData_DE = "SmallExportTestData_DE.csv";
    private const string PathToSmallData_Can = "SmallExportTestData_Can.csv";

    [Fact]
    public void loadExportData_smallCsv_builds_object_graph()
    {
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData, Commodity.OilSeed);

        Assert.Equal(6, target.ExportDataSet.Count);
        Assert.Equal(6, target.CountryDataSet.Count);

        Assert.All(target.ExportDataSet, exportData =>
        {
            Assert.NotNull(exportData.Country);
            Assert.NotNull(exportData.Country.Continent);
            Assert.Contains(exportData.Country, exportData.Country.Continent.Countries);
        });

        var europeanCountries = target.CountryDataSet.Where(c => c.Continent.Id == "eu").ToList();
        Assert.Equal(3, europeanCountries.Count);
        Assert.Single(europeanCountries.Select(c => c.Continent).Distinct());
    }

    [Fact]
    public void loadExportData_multipleFiles_keeps_commodity_and_graph_integrity()
    {
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData_DE, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Can, Commodity.Wheat);

        Assert.Equal(2, target.ExportDataSet.Count);
        Assert.Equal(1, target.ExportDataSet.Count(x => x.Commodity == Commodity.OilSeed));
        Assert.Equal(1, target.ExportDataSet.Count(x => x.Commodity == Commodity.Wheat));
        Assert.Equal(0, target.ExportDataSet.Count(x => x.Commodity == Commodity.Unknown));

        Assert.All(target.ExportDataSet, exportData =>
        {
            Assert.NotNull(exportData.Country);
            Assert.NotNull(exportData.Country.Continent);
            Assert.Contains(exportData.Country, exportData.Country.Continent.Countries);
        });
    }
}