namespace Foundation;

using Foundation.Collections;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

public abstract class Either
{
    public Either([DisallowNull] IEnumerable alternatives)
    {
        Alternatives = alternatives.ThrowIfNull(nameof(alternatives)).ToObjectArray();
    }

    protected object?[] Alternatives { get; }

    public int Count => Alternatives.Length;

    public bool Is<T>() => Alternatives.FirstOrDefault() is T;

    public IEnumerable<T> GetAlternatives<T>()
    {
        foreach (var alternative in Alternatives)
        {
            if (alternative is T t) yield return t;
        }
    }
}


public class Either<T1, T2> : Either
{
    public Either(params T1[] alternatives) : base(alternatives)
    {
    }

    public Either(params T2[] alternatives) : base(alternatives)
    {
    }
}

public class Either<T1, T2, T3> : Either
{
    public Either(params T1[] alternatives) : base(alternatives) { }
    public Either(params T2[] alternatives) : base(alternatives) { }
    public Either(params T3[] alternatives) : base(alternatives) { }
}

public class Either<T1, T2, T3, T4> : Either
{
    public Either(params T1[] alternatives) : base(alternatives) { }
    public Either(params T2[] alternatives) : base(alternatives) { }
    public Either(params T3[] alternatives) : base(alternatives) { }
    public Either(params T4[] alternatives) : base(alternatives) { }
}

public class Either<T1, T2, T3, T4, T5> : Either
{
    public Either(params T1[] alternatives) : base(alternatives) { }
    public Either(params T2[] alternatives) : base(alternatives) { }
    public Either(params T3[] alternatives) : base(alternatives) { }
    public Either(params T4[] alternatives) : base(alternatives) { }
    public Either(params T5[] alternatives) : base(alternatives) { }
}
