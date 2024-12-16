namespace Shared;

public class Vector2T<T>(T x, T y) : IEquatable<Vector2T<T>>
{
    public T X = x;
    public T Y = y;

    public bool Equals(Vector2T<T> other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2T<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}

public class Vector2i(int x, int y) : Vector2T<int>(x, y)
{
    
}

public class Vector2u(uint x, uint y) : Vector2T<uint>(x, y)
{
    
}

public class Vector2ul(ulong x, ulong y) : Vector2T<ulong>(x, y)
{
    
}

public class Vector2l(long x, long y) : Vector2T<long>(x, y)
{
    
}