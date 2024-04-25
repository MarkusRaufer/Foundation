using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Foundation.Linq.Expressions;

public static class IdExpression
{
    public static IdExpression<Guid, TExpression> New<TExpression>(TExpression expression)
        where TExpression : Expression => new(Guid.NewGuid(), expression);

    public static IdExpression<TId, TExpression> New<TId, TExpression>(TId id, TExpression expression)
        where TExpression : Expression
        where TId : notnull => new(id, expression);
}

public class IdExpression<TExpression> : Expression
    where TExpression : Expression
{
    public IdExpression(TExpression expression)
    {
        Expression = expression.ThrowIfNull();
    }

    public static implicit operator TExpression(IdExpression<TExpression> idExpression) => idExpression.Expression;

    public TExpression Expression { get; }

    public override ExpressionType NodeType => Expression.NodeType;

    public override string ToString() => $"Expression: {Expression}";
}

public class IdExpression<TId, TExpression>
    : IdExpression<TExpression>
    , IIdentifiable<TId>
    , IEquatable<IdExpression<TId, TExpression>>
    where TExpression : Expression
    where TId : notnull
{
    public IdExpression(TId id, TExpression expression) : base(expression)
    {
        Id = id.ThrowIfNull();
    }

    public static bool operator ==(IdExpression<TId, TExpression> lhs, IdExpression<TId, TExpression> rhs)
    {
        if (lhs is null) return rhs is null;
        if (rhs is null) return false;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(IdExpression<TId, TExpression> lhs, IdExpression<TId, TExpression> rhs) => !(lhs == rhs);

    public static implicit operator TExpression(IdExpression<TId, TExpression> idExpression) => idExpression.Expression;

    public override bool Equals(object? obj) => obj is IdExpression<TId, TExpression> other && Equals(other);

    public bool Equals(IdExpression<TId, TExpression>? other) => other is not null && Id.Equals(other.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public TId Id { get; }

    public override ExpressionType NodeType => Expression.NodeType;

    public override string ToString() => $"Id: {Id}, Expression: {Expression}";
}
