using ExportAnalyzer.Entities;

namespace ExportAnalyzer;

public static class ExportDataExtensions {
    public static IEnumerable<ExportData> WhereCommodity(this IEnumerable<ExportData> ed, Commodity commodity)
    {
        return ed.Where(x => x.Commodity == commodity);
    }
}