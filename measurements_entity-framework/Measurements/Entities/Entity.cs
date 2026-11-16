namespace Measurements.Entities;

public abstract class Entity
{
    public required int Id { get; set; }

    public override bool Equals(object? obj) =>
        obj is Entity { Id: var otherId } other
        && GetType() == other.GetType()
        && Id == otherId;

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity? left, Entity? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);
}
