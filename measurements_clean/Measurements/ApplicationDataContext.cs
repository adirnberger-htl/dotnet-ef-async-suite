using System.Globalization;
using Measurements.Entities;

namespace Measurements;

public partial class ApplicationDataContext
{
    public ISet<Measurement> MeasurementDataSet { get; private set; }
    public ISet<Sensor> SensorDataSet { get; private set; }
    public ISet<MeasurementType> MeasurementTypeDataSet { get; private set; }
    public ISet<SensorType> SensorTypeDataSet { get; private set; }
    private static readonly string Seperator = ";";

    public ApplicationDataContext()
    {
        MeasurementDataSet = new HashSet<Measurement>();
        SensorDataSet = new HashSet<Sensor>();
        MeasurementTypeDataSet = new HashSet<MeasurementType>();
        SensorTypeDataSet = new HashSet<SensorType>();
    }

    public void LoadData(string fileSensorType, string fileSensor, string fileMeasurementType, string fileMeasurement)
    {
        try
        {
            LoadData(File.ReadAllLines(fileSensorType), File.ReadAllLines(fileSensor),
                File.ReadAllLines(fileMeasurementType), File.ReadAllLines(fileMeasurement));
        }
        catch (IOException e)
        {
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("Error: " + e.Message);
            throw;
        }

    }

    public void LoadData(string[] linesSensorType, string[] linesSensor, string[] linesMeasurementType, string[] linesMeasurement)
    {
        ClearData();
        LoadSensorTypeData(linesSensorType);
        LoadSensorData(linesSensor);
        LoadMeasurementTypeData(linesMeasurementType);
        LoadMeasurementData(linesMeasurement);
    }

    private static IEnumerable<TOut> ParseCsv<TOut>(IEnumerable<string> lines, Func<string[], TOut> selector, int skip = 1) =>
        lines.Skip(skip).Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => selector(l.Split(Seperator)));

    private static int ParseInt(string s) => int.Parse(s, CultureInfo.InvariantCulture);
    private static double ParseDouble(string s) => double.Parse(s, CultureInfo.InvariantCulture);
    private static DateTime ParseDateTime(string s) => DateTime.Parse(s, CultureInfo.InvariantCulture);

    private void LoadSensorTypeData(string[] lines) =>
        SensorTypeDataSet = ParseCsv(lines, p => new SensorType { Id = ParseInt(p[0]), Name = p[1] }).ToHashSet();

    private void LoadSensorData(string[] lines)
    {
        var sensorTypes = SensorTypeDataSet.ToDictionary(st => st.Id);
        SensorDataSet = ParseCsv(lines, p => new Sensor
        {
            Id = ParseInt(p[0]),
            Name = p[1],
            FirstSeen = ParseDateTime(p[2]),
            SensorTypeId = ParseInt(p[3]),
            SensorType = sensorTypes[ParseInt(p[3])]
        }).ToHashSet();

        foreach (var sensor in SensorDataSet)
        {
            sensor.SensorType.Sensors.Add(sensor);
        }
    }

    private void LoadMeasurementTypeData(string[] lines) =>
        MeasurementTypeDataSet = ParseCsv(lines, p => new MeasurementType { Id = ParseInt(p[0]), Name = p[1], Comment = p.ElementAtOrDefault(2) ?? string.Empty }).ToHashSet();

    private void LoadMeasurementData(string[] lines)
    {
        var sensors = SensorDataSet.ToDictionary(s => s.Id);
        var measurementTypes = MeasurementTypeDataSet.ToDictionary(mt => mt.Id);
        MeasurementDataSet = ParseCsv(lines, p => new Measurement
        {
            Id = ParseInt(p[0]),
            Value = ParseDouble(p[1]),
            Time = ParseDateTime(p[2]),
            SensorId = ParseInt(p[3]),
            MeasurementTypeId = ParseInt(p[4]),
            Sensor = sensors[ParseInt(p[3])],
            MeasurementType = measurementTypes[ParseInt(p[4])]
        }).ToHashSet();

        foreach (var measurement in MeasurementDataSet)
        {
            measurement.Sensor.Measurements.Add(measurement);
            measurement.MeasurementType.Measurements.Add(measurement);
        }
    }

    public void ClearData()
    {
        MeasurementDataSet.Clear();
        MeasurementTypeDataSet.Clear();
        SensorDataSet.Clear();
        SensorTypeDataSet.Clear();
    }
}
