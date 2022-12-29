namespace Foundation;

public interface IResult : IResult<Exception>
{
}

public interface IResult<TError>
{
    bool IsOk { get; }
    bool TryGetError(out TError? error);
}

public interface IResult<TOk, TError> : IResult<TError>
{
    bool TryGetOk(out TOk? ok);
}
