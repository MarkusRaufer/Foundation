namespace Foundation;

public interface IResult<TError> : IResult<bool, TError>
{
}

public interface IResult<out TOk, out TError>// : IBooleanResult<TError>
{
    TOk Ok { get; }
}
