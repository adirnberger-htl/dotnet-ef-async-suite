namespace Measurements.Entities;

public class MeasurementType : Entity
{
    public required string Name { get; set; } = string.Empty;
    public required string Comment { get; set; } = string.Empty;

    public ICollection<Measurement> Measurements { get; private set; } = [];
}
