namespace Shared;

public struct Vector2i(int x, int y) : IEquatable<Vector2i>
{
    public int X = x;
    public int Y = y;

    public bool Equals(Vector2i other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2i other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}