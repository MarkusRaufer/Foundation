namespace Foundation;

public sealed class Number : OneOf<byte, char, decimal, int, long, sbyte, short, uint, ulong, ushort>
{
    public Number()
    {
    }

    public Number(byte t1) : base(t1)
    {
    }

    public Number(char t2) : base(t2)
    {
    }

    public Number(decimal t3) : base(t3)
    {
    }

    public Number(int t4) : base(t4)
    {
    }

    public Number(long t5) : base(t5)
    {
    }

    public Number(sbyte t6) : base(t6)
    {
    }

    public Number(short t7) : base(t7)
    {
    }

    public Number(uint t8) : base(t8)
    {
    }

    public Number(ulong t9) : base(t9)
    {
    }

    public Number(ushort t10) : base(t10)
    {
    }

    public static implicit operator Number(byte number) => new(number);
    public static implicit operator Number(char number) => new(number);
    public static implicit operator Number(decimal number) => new(number);
    public static implicit operator Number(int number) => new(number);
    public static implicit operator Number(long number) => new(number);
    public static implicit operator Number(sbyte number) => new(number);
    public static implicit operator Number(short number) => new(number);
    public static implicit operator Number(uint number) => new(number);
    public static implicit operator Number(ulong number) => new(number);
    public static implicit operator Number(ushort number) => new(number);

    public static Number New(byte number) => new (number);
    public static Number New(char number) => new(number);
    public static Number New(decimal number) => new (number);
    public static Number New(int number) => new (number);
    public static Number New(long number) => new (number);
    public static Number New(sbyte number) => new(number);
    public static Number New(short number) => new (number);
    public static Number New(uint number) => new(number);
    public static Number New(ulong number) => new(number);
    public static Number New(ushort number) => new (number);
}
