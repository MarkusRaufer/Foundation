namespace Foundation;

public interface INumeric<T>
    where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
{
    T Add(T a, T b);
    T Divide(T a, T b);
    T Multiply(T a, T b);
    T Subtract(T a, T b);
}

public struct NumByte : INumeric<byte>
{
    public byte Add(byte a, byte b) => (byte)(a + b);

    public byte Divide(byte a, byte b) => (byte)(a / b);

    public byte Multiply(byte a, byte b) => (byte)(a * b);

    public byte Subtract(byte a, byte b) => (byte)(a - b);
}

public struct NumDecimal : INumeric<decimal>
{
    public decimal Add(decimal a, decimal b) => a + b;

    public decimal Divide(decimal a, decimal b) => a / b;

    public decimal Multiply(decimal a, decimal b) => a * b;

    public decimal Subtract(decimal a, decimal b) => a - b;
}

public struct NumDouble : INumeric<double>
{
    public double Add(double a, double b) => a + b;

    public double Divide(double a, double b) => a / b;

    public double Multiply(double a, double b) => a * b;

    public double Subtract(double a, double b) => a - b;

}

public struct NumFloat : INumeric<float>
{
    public float Add(float a, float b) => a + b;

    public float Divide(float a, float b) => a / b;

    public float Multiply(float a, float b) => a * b;

    public float Subtract(float a, float b) => a - b;
}

public struct NumInt : INumeric<int>
{
    public int Add(int a, int b) => a + b;

    public int Divide(int a, int b) => a / b;

    public int Multiply(int a, int b) => a * b;

    public int Subtract(int a, int b) => a - b;
}

public struct NumLong : INumeric<long>
{
    public long Add(long a, long b) => a + b;

    public long Divide(long a, long b) => a / b;

    public long Multiply(long a, long b) => a * b;

    public long Subtract(long a, long b) => a - b;
}

public struct NumShort : INumeric<short>
{
    public short Add(short a, short b) => (short)(a + b);

    public short Divide(short a, short b) => (short)(a / b);

    public short Multiply(short a, short b) => (short)(a * b);

    public short Subtract(short a, short b) => (short)(a - b);
}

