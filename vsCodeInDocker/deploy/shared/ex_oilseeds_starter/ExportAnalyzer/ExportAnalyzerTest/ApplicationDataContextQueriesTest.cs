using System;
using System.Collections.Generic;
using System.Linq;
using ExportAnalyzer;
using ExportAnalyzer.Entities;
using Xunit;

namespace ExportAnalyzerTest;

public class ApplicationDataContextQueriesTest
{
    private const string PathToSeedOilData = "Exporters-of-Seed-Oils-2020-Click-to-Select-a-Country.csv";
    private const string PathToWheatData = "Exporters-of-Wheat-2020-Click-to-Select-a-Country.csv";
    private const string PathToSmallData = "SmallExportTestData.csv";
    private const string PathToSmallData_DE = "SmallExportTestData_DE.csv";
    private const string PathToSmallData_Col = "SmallExportTestData_Col.csv";
    private const string PathToSmallData_Can = "SmallExportTestData_Can.csv";
    private const string PathToStatisticsTestData = "StatisticsTestData.csv";
    
    [Fact]
    public void oilSeedData_GetExportingContinentsTest()
    {
        var expected = (new List<string> { "Africa", "Asia", "Europa", "South America", "North America", "Oceania"});
        expected.Sort();
        
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSeedOilData, Commodity.OilSeed);
        Assert.True(target.ExportDataSet.Count > 0);
        var result = target.GetExportingContinents().OrderBy(x => x).ToList();

        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void smallData_GetAllCountriesTest()
    {
        var expected = (new List<string> { "Czechia", "Germany", "Denmark", "Colombia", "Ecuador", "Guyana"});
        expected.Sort();
        
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData, Commodity.OilSeed);
        Assert.True(target.ExportDataSet.Count > 0);
        var result = target.GetAllCountriesOffering(Commodity.OilSeed).OrderBy(x => x).ToList();
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void smallData_GetAllCountries_DifferentCommodity_Test()
    {
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData, Commodity.OilSeed);
        Assert.True(target.ExportDataSet.Count > 0);
        var result = target.GetAllCountriesOffering(Commodity.Wheat).OrderBy(x => x).ToList();
       Assert.True(result.Count == 0);
    }
    
    [Fact]
    public void smallData_GetAllCountries_ContinentEU_Test()
    {
        var expected = (new List<string> { "Czechia", "Germany", "Denmark"});
        expected.Sort();
        
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData, Commodity.OilSeed);
        Assert.True(target.ExportDataSet.Count > 0);
        var result = target.GetAllCountriesOnContinent("eu").ToList();
        result.Sort();
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void smallData_SeedOil_GetCommoditiesWithData_WheatAndOilSeed()
    {
        var expected = (new List<Commodity> {Commodity.Wheat, Commodity.OilSeed});
        expected.Sort();
        
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData, Commodity.Wheat);
        target.LoadExportData(PathToSeedOilData, Commodity.OilSeed);
        Assert.True(target.ExportDataSet.Count > 0);
        
        var result = target.GetAvailableCommodities().ToList();
        result.Sort();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void smallData_GetCountriesProducing_OneCommodity_Test()
    {
        var expected = new List<string> {"Germany"};
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData_DE, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Col, Commodity.Wheat);
        target.LoadExportData(PathToSmallData_Can, Commodity.Metal);

        var result = target.GetCountriesProducing(new[] {Commodity.OilSeed}, true).ToList();
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void smallData_GetCountriesProducing_TwoCommodities_Test()
    {
        var expected = new List<string> {"Germany", "Canada"};
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData_DE, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Col, Commodity.Wheat);
        target.LoadExportData(PathToSmallData_Can, Commodity.Metal);

        var result = target.GetCountriesProducing(new[] { Commodity.OilSeed, Commodity.Metal }, true).ToList();
        expected.Sort();
        result.Sort();
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void smallData_GetCountriesProducing_TwoCommodities_All_Test()
    {
        var expected = new List<string> {"Germany"};
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData_DE, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_DE, Commodity.Wheat);
        target.LoadExportData(PathToSmallData_Col, Commodity.Wheat);
        target.LoadExportData(PathToSmallData_Can, Commodity.Metal);

        var result = target.GetCountriesProducing(new[] { Commodity.OilSeed, Commodity.Wheat }, false).ToList();
        expected.Sort();
        result.Sort();
        Assert.Equal(expected, result);
        
        var result1 = target.GetCountriesProducing(new[] { Commodity.OilSeed, Commodity.Metal }, false).ToList();
        Assert.Empty(result1);
    }
    
    [Fact]
    public void smallData_GetSmallestExporter_Test()
    {
        var expected = "Colombia";
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData_DE, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Can, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Col, Commodity.OilSeed);

        var result = target.GetSmallestExporterOf(Commodity.OilSeed);
        Assert.Equal(expected, result.Country.Name);
    }
    
    [Fact]
    public void smallData_GetBiggestExporter_Test()
    {
        var expected = "Germany";
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData_DE, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Can, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Col, Commodity.OilSeed);

        var result = target.GetBiggestExporterOf(Commodity.OilSeed);
        Assert.Equal(expected, result.Country.Name);
    }
    
    [Fact]
    public void smallData_GetTopExporters_Test()
    {
        var expected = new List<string> {"Germany", "Canada"};
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData_DE, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Can, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Col, Commodity.OilSeed);

        var result = target.GetTopExporters(Commodity.OilSeed, 2).Select(x => x.Country.Name).ToList();
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void smallData_GetTopExportersPerContinent_Test()
    {
        var expected = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("eu", "Germany"), 
            new Tuple<string, string>("na", "Canada"),
            new Tuple<string, string>("sa", "Colombia")
        };
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSmallData_DE, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Can, Commodity.OilSeed);
        target.LoadExportData(PathToSmallData_Col, Commodity.OilSeed);

        var result = target.GetTopExportersPerContinent(Commodity.OilSeed, 2)
            .Select(x => new Tuple<string, string>(x.Key, x.Value.First().Country.Name))
            .ToList();
        result.Sort();
        expected.Sort();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void oilSeedData_GetTopExportersPerContinent_Test()
    {
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToSeedOilData, Commodity.OilSeed);

        var result = target.GetTopExportersPerContinent(Commodity.OilSeed, 3);
        Assert.True(result.Count > 0);
    }
    
    [Fact]
    public void oilSeedData_GetExportStatisticsPerContinent_Test()
    {
        var target = new ApplicationDataContext();
        target.LoadExportData(PathToStatisticsTestData, Commodity.OilSeed);

        var result = target.GetExportStatisticsPerContinent(Commodity.OilSeed);
        
        //there is 1 continents in the data
        Assert.Single(result);
        var sam = result["af"];
        
        Assert.Equal(33, sam.Sum);
        Assert.InRange(sam.Average, 2, 4);
        Assert.InRange(sam.Stddev, 0, 2);
    }
}