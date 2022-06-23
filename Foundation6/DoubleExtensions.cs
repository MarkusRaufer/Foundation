namespace Foundation;

public static class DoubleExtensions
{
    public static bool IsZero(this double value, double epsilon = double.Epsilon)
    {
        return Math.Abs(value) < double.Epsilon;
    }

    public static bool Equal(this double left, double right, double epsilon = double.Epsilon)
    {
        return (Math.Abs(left) - Math.Abs(right)) < epsilon;
    }

    public static bool GreaterThan(this double left, double right, double epsilon = double.Epsilon)
    {
        return (left - right) > epsilon;
    }

    public static bool GreaterThanOrEqual(this double left, double right, double epsilon = double.Epsilon)
    {
        return Equal(left, right, epsilon) || GreaterThan(left, right, epsilon);
    }

    public static bool LessThan(this double left, double right, double epsilon = double.Epsilon)
    {
        return (left - right) < -epsilon;
    }

    public static bool LessThanOrEqual(this double left, double right, double epsilon = double.Epsilon)
    {
        return Equal(left, right, epsilon) || LessThan(left, right, epsilon);
    }
}
