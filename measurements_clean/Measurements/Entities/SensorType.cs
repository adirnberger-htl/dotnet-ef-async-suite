namespace Measurements.Entities;

public class SensorType : Entity
{
    public required string Name { get; set; } = string.Empty;

    public ICollection<Sensor> Sensors { get; private set; } = [];
}
