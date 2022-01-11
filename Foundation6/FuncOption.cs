namespace Foundation;

using System.Diagnostics;

public static class FuncOption
{
    public static NoneFunc<T> None<T>()
    {
        return new NoneFunc<T>();
    }

    public static NoneFunc<T> None<T>(T value)
    {
        return new NoneFunc<T>(value);
    }

    public static SomeFunc<T> Some<T>(T value, bool isCalled = false)
    {
        return new SomeFunc<T>(value, isCalled);
    }
}

[DebuggerDisplay("HasValue={HasValue}, Value={Value}, IsCalled={IsCalled}")]
public abstract class FuncOption<T>
{
    protected FuncOption()
    {
    }

    protected FuncOption(T? value, bool isCalled = false)
    {
        if (null == value) return;

        Value = value;
        HasValue = true;
        IsCalled = isCalled;
    }

    public bool HasValue { get; protected set; }
    public bool IsCalled { get; protected set; }
    public T? Value { get; private set; }
}

[DebuggerDisplay("HasValue={HasValue}, Value={Value}")]
public class NoneFunc<T> : FuncOption<T>
{
    public NoneFunc()
    {
    }

    public NoneFunc(T? value)
        : base(value)
    {
    }
}

[DebuggerDisplay("HasValue={HasValue}, Value={Value}, IsCalled={IsCalled}")]
public class SomeFunc<T> : FuncOption<T>
{
    public SomeFunc(T? value, bool isCalled = false)
        : base(value, isCalled)
    {
    }
}
