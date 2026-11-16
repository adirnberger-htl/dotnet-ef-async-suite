namespace Measurements;

public record SensorMeasurementCount(int Id, string Name, int Count);

public record MeasurementTypeMeasurementCount(int Id, string Name, int Count);

public record SensorMeasurementTypeStatistic(string SensorName, string MeasurementTypeName, int Count, double Min, double Max);

public partial class ApplicationDataContext
{
    public IList<SensorMeasurementCount> GetSensorsWithMeasurementCount() =>
        SensorDataSet
            .OrderBy(s => s.Id)
            .Select(s => new SensorMeasurementCount(s.Id, s.Name, s.Measurements.Count))
            .ToList();

    public IList<MeasurementTypeMeasurementCount> GetMeasurementTypesWithMeasurementCount() =>
        MeasurementTypeDataSet
            .OrderBy(mt => mt.Id)
            .Select(mt => new MeasurementTypeMeasurementCount(mt.Id, mt.Name, mt.Measurements.Count))
            .ToList();

    public IList<SensorMeasurementTypeStatistic> GetMeasurementStatisticsBySensorAndType() =>
        MeasurementDataSet
            .GroupBy(m => new { SensorName = m.Sensor.Name, MeasurementTypeName = m.MeasurementType.Name })
            .Select(g => new SensorMeasurementTypeStatistic(
                g.Key.SensorName,
                g.Key.MeasurementTypeName,
                g.Count(),
                g.Min(m => m.Value),
                g.Max(m => m.Value)))
            .OrderBy(s => s.SensorName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(s => s.MeasurementTypeName, StringComparer.OrdinalIgnoreCase)
            .ToList();
}
