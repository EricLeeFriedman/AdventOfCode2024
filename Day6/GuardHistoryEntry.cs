using Shared;

namespace Day6;

public struct GuardHistoryEntry : IEquatable<GuardHistoryEntry>
{
    public Vector2i Location;
    public Vector2i Direction;

    public bool Equals(GuardHistoryEntry other)
    {
        return Location.Equals(other.Location) && Direction.Equals(other.Direction);
    }

    public override bool Equals(object? obj)
    {
        return obj is GuardHistoryEntry other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Location, Direction);
    }
}