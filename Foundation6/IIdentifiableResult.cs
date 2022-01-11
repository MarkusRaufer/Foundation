namespace Foundation;

public interface IIdentifiableResult<TOk> : IIdentifiableResult<TOk, Exception>
{
}

public interface IIdentifiableResult<TOk, TError> : IIdentifiableResult<Guid, TOk, TError>
{
}

public interface IIdentifiableResult<TId, TOk, TError>
    : IIdentifiable<TId>
    , IResult<TOk, TError>
    where TId : notnull
{
}
