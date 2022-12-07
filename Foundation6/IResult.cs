namespace Foundation;

public interface IResult : IResult<Exception>
{
}

public interface IResult<out TError>
{
    TError Error { get; }
    bool IsOk { get; }
}

public interface IResult<out TOk, out TError> : IResult<TError>
{
    TOk Ok { get; }
}
