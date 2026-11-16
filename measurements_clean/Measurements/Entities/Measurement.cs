namespace Measurements.Entities;

public class Measurement : Entity
{
    public double Value { get; set; }
    public DateTime Time { get; set; }
    public int SensorId { get; set; }
    public int MeasurementTypeId { get; set; }

    public required MeasurementType MeasurementType { get; set; }
    public required Sensor Sensor { get; set; }
}