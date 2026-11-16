namespace Measurements.Entities;

public class Sensor : Entity
{
    public required string Name { get; set; } = string.Empty;
    public DateTime FirstSeen { get; set; }
    public int SensorTypeId { get; set; }
    public required SensorType SensorType { get; set; }

    public ICollection<Measurement> Measurements { get; private set; } = [];
}
