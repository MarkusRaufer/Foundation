namespace Foundation;

public interface IBooleanResult : IBooleanResult<Exception>
{
}

public interface IBooleanResult<out TError>
{
    TError Error { get; }
    bool IsError { get; }
    bool IsOk { get; }
}
