using ExportAnalyzer.Entities;
using System.Linq;

namespace ExportAnalyzer;

//TODO Implement the queries. Use Linq as much as possible!

public partial class ApplicationDataContext
{
    public IEnumerable<string> GetExportingContinents()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetAllCountriesOnContinent(string continentId)
    {
        throw new NotImplementedException();
    }
   
    public IEnumerable<string> GetAllCountriesOffering(Commodity commodity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Commodity> GetAvailableCommodities()
    {
        throw new NotImplementedException();
    }

    //any = true: Report country that produces at least one of the given commodities.
    //any = false: Report country that produces all of the given commodities.
    public IEnumerable<string> GetCountriesProducing(IEnumerable<Commodity> commodities, bool any)
    {
        throw new NotImplementedException();
    }

    public ExportData GetSmallestExporterOf(Commodity commodity)
    {
        throw new NotImplementedException();
    }

    public ExportData GetBiggestExporterOf(Commodity commodity)
    {
        throw new NotImplementedException();
    }

    public List<ExportData> GetTopExporters(Commodity commodity, int topN = 10)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, IEnumerable<ExportData>> GetTopExportersPerContinent(Commodity commodity, int topN = 5)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, ExportStatistics> GetExportStatisticsPerContinent(Commodity commodity)
    {
        throw new NotImplementedException();
    }

    private ExportStatistics CalculateExportStatistics(IEnumerable<ExportData> exportData)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, double> GetExportValuePerPersonPerContinent(Commodity commodity)
    {
        throw new NotImplementedException();
    }
}