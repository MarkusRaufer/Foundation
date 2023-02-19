namespace Foundation;

public static class DoubleExtensions
{
    public static bool IsZero(this double value, double deviation = double.Epsilon)
    {
        return Math.Abs(value) < deviation;
    }

    public static bool Equal(this double left, double right, double deviation = double.Epsilon)
    {
        return (Math.Abs(left) - Math.Abs(right)) < deviation;
    }

    public static bool GreaterThan(this double left, double right, double deviation = double.Epsilon)
    {
        return (left - right) > deviation;
    }

    public static bool GreaterThanOrEqual(this double left, double right, double deviation = double.Epsilon)
    {
        return Equal(left, right, deviation) || GreaterThan(left, right, deviation);
    }

    public static bool LessThan(this double left, double right, double deviation = double.Epsilon)
    {
        return (left - right) < -deviation;
    }

    public static bool LessThanOrEqual(this double left, double right, double deviation = double.Epsilon)
    {
        return Equal(left, right, deviation) || LessThan(left, right, deviation);
    }
}
