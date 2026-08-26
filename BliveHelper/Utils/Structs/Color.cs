using System;
using System.Diagnostics.CodeAnalysis;

namespace BliveHelper.Utils.Structs;

public struct Color(byte r, byte g, byte b, byte a = 255) : IEquatable<Color>
{
    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color White = new(255, 255, 255);

    public byte R { get; set; } = r;
    public byte G { get; set; } = g;
    public byte B { get; set; } = b;
    public byte A { get; set; } = a;

    public override readonly bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is Color other && Equals(other);
    }

    public readonly bool Equals(Color other)
    {
        return R == other.R && G == other.G && B == other.B && A == other.A;
    }

    public override readonly int GetHashCode()
    {
        var hash = 17L;
        hash = hash * 31 + R.GetHashCode();
        hash = hash * 31 + G.GetHashCode();
        hash = hash * 31 + B.GetHashCode();
        hash = hash * 31 + A.GetHashCode();
        return (int)hash;
    }
}
