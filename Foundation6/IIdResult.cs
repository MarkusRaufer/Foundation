namespace Foundation;

public interface IIdResult<TId, TOk, TError>
    : IResult<TOk, TError>
    , IIdentifiable<TId>
{
}
