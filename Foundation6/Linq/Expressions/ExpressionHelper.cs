using Foundation;
using Foundation.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Linq.Expressions;

public static class ExpressionHelper
{
    //public static bool AreEqual(Expression lhs, Expression rhs)
    //{
    //    lhs.ThrowIfNull();
    //    rhs.ThrowIfNull();

    //    return AreEqual(lhs.Flatten(), rhs.Flatten());
    //}

    //public static bool AreEqual(IEnumerable<Expression> lhs, IEnumerable<Expression> rhs)
    //{
    //    var flatLhs = lhs.ToArray();
    //    var flatRhs = rhs.ToArray();

    //    if (flatLhs.Length != flatRhs.Length) return false;

    //    foreach (var (l, r) in flatLhs.Zip(flatRhs, (l, r) => (l, r)))
    //    {
    //        if (l.NodeType != r.NodeType) return false;
    //        if (l.Type != r.Type) return false;

    //        if (ExpressionType.Constant == l.NodeType)
    //        {
    //            if (l is not ConstantExpression lhsConstant || r is not ConstantExpression rhsConstant)
    //                return false;

    //            if (!lhsConstant.EqualsToExpression(rhsConstant)) return false;

    //            continue;
    //        }

    //        if (ExpressionType.Lambda == l.NodeType)
    //        {
    //            if (l is not LambdaExpression lhsLambda || r is not LambdaExpression rhsLambda) return false;

    //            if (!lhsLambda.EqualsToExpression(rhsLambda)) return false;

    //            continue;
    //        }

    //        if (ExpressionType.MemberAccess == l.NodeType)
    //        {
    //            if (l is not MemberExpression lhsMember || r is not MemberExpression rhsMember) return false;

    //            if (!lhsMember.EqualsToExpression(rhsMember)) return false;

    //            continue;
    //        }

    //        if (ExpressionType.Parameter == l.NodeType)
    //        {
    //            if (l is not ParameterExpression lhsParameter || r is not ParameterExpression rhsParameter)
    //                return false;

    //            if (!lhsParameter.EqualsToExpression(rhsParameter)) return false;

    //            continue;
    //        }
    //    }
    //    return true;
    //}

    public static bool AreEqualTerminators(Expression lhs, Expression rhs)
    {
        return AreEqualTerminators(lhs, rhs, false);
    }

    public static bool AreEqualTerminators(Expression lhs, Expression rhs, bool same)
    {
        if (lhs.NodeType != rhs.NodeType) return false;
        if (lhs.Type != rhs.Type) return false;

        return lhs switch
        {
            ConstantExpression l => rhs is ConstantExpression r && (same ? ReferenceEquals(l, r) : l.EqualsToExpression(r)),
            MemberExpression l => rhs is MemberExpression r && (same ? ReferenceEquals(l, r) : l.EqualsToExpression(r)),
            ParameterExpression l => rhs is ParameterExpression r && (same ? ReferenceEquals(l, r) : l.EqualsToExpression(r)),
            _ => false
        };
    }

    public static bool AreSameTerminators(Expression lhs, Expression rhs)
    {
        return AreEqualTerminators(lhs, rhs, true);
    }

    public static BinaryExpression? Concat(IEnumerable<BinaryExpression> expressions, ExpressionType binaryType)
    {
        return expressions.Aggregate(x => x, (acc, x) => Expression.MakeBinary(binaryType, acc, x))
                          .TryGet(out var binaryExpression) ? binaryExpression : null;
    }

    //public static Expression<Func<T, object>> ConvertToReturnTypeObject<T>(MemberExpression member)
    //{
    //    var objectMember = Expression.Convert(member, typeof(object));

    //    return member.Expression is ParameterExpression parameter
    //        ? Expression.Lambda<Func<T, object>>(objectMember, parameter)
    //        : Expression.Lambda<Func<T, object>>(objectMember);
    //}

    //public static Expression<Func<T, object>> ConvertToReturnTypeObject<T, TMember>(Expression<Func<T, TMember>> expression)
    //{
    //    var converted = Expression.Convert(expression.Body, typeof(object));
    //    return Expression.Lambda<Func<T, object>>(converted, expression.Parameters);
    //}

    //public static int CreateHashCode(this Expression expression)
    //{
    //    return CreateHashCode(expression.Flatten());
    //}

    //public static int CreateHashCode(IEnumerable<Expression> expressions)
    //{
    //    return HashCode.FromOrderedHashCode(expressions.Select(x => x.GetExpressionHashCode()).ToArray());
    //}

    //public static IEnumerable<(int hashcode, Expression expression)> CreateHashCodeTuples(
    //    IEnumerable<Expression> expressions)
    //{
    //    foreach (var expr in expressions)
    //    {
    //        var builder = HashCode.CreateBuilder();

    //        switch (expr)
    //        {
    //            case BinaryExpression be:
    //                if (ExpressionType.Modulo == be.NodeType) break;

    //                builder.AddObject(be.NodeType);
    //                builder.AddObject(be.Type);

    //                yield return (builder.GetHashCode(), be);
    //                break;
    //            case ConstantExpression ce:
    //                builder.AddHashCode(CreateHashCode(ce));

    //                yield return (builder.GetHashCode(), ce);
    //                break;
    //            case LambdaExpression le:
    //                builder.AddObject(le.NodeType);
    //                builder.AddObject(le.Type);
    //                builder.AddObject(le.ReturnType);

    //                yield return (builder.GetHashCode(), le);
    //                break;
    //            case MemberExpression me:
    //                builder.AddHashCode(CreateHashCode(me));
    //                yield return (builder.GetHashCode(), me);
    //                break;
    //            case ParameterExpression pe:
    //                builder.AddHashCode(CreateHashCode(pe));
    //                yield return (builder.GetHashCode(), pe);
    //                break;
    //            case UnaryExpression ue:
    //                builder.AddHashCode(CreateHashCode(ue));
    //                yield return (builder.GetHashCode(), ue);
    //                break;
    //        };
    //    }
    //}


    public static string ExpressionToString(Expression expression, bool addSpace = false)
    {
        return ExpressionTypeToString(expression.NodeType, addSpace);
    }

    public static string ExpressionTypeToString(ExpressionType expressionType, bool addSpace = false)
    {
        var strType = expressionType.ToCsharpString();

        return addSpace ? $" {strType} " : strType;
    }

    /// <summary>
    /// Returns all MemberExpressions of a BinaryExpression. It checks Left and Right.
    /// Can be one, two or empty.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IEnumerable<MemberExpression> GetMemberExpressions(BinaryExpression expression)
    {
        if (expression.Left is MemberExpression left) yield return left;
        if (expression.Right is MemberExpression right) yield return right;
    }

    public static MemberInfo GetMemberInfo(Expression expression)
    {
        expression.ThrowIfNull();

        MemberExpression? me;
        if (expression is LambdaExpression lambda)
        {
            me = lambda.Body as MemberExpression;
            if (null == me)
            {
                if (lambda.Body is UnaryExpression unary)
                    me = unary.Operand as MemberExpression;
            }
        }
        else
            me = expression as MemberExpression;

        if (null == me)
            throw new ArgumentOutOfRangeException(nameof(expression), "expression is not a member expression");

        return me.Member;
    }

    public static MemberInfo GetMemberInfoFromLambda(Expression expression)
    {
        expression.ThrowIfNull();

        if (expression is not LambdaExpression lambda)
            throw new ArgumentOutOfRangeException(nameof(expression), "expression is not a lambda");

        if (lambda.Body is not MemberExpression me)
            throw new ArgumentOutOfRangeException(nameof(expression), "expression is not a member expression");

        return me.Member;
    }

    /// <summary>
    /// Returns a ParameterExpression. It checks Left and Right.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns>A ParameterExpression or null.</returns>
    public static IEnumerable<ParameterExpression> GetParameterExpressions(Expression expression)
    {
        expression.ThrowIfNull();

        if (expression is LambdaExpression lambda)
            expression = lambda.Body;

        if (expression is BinaryExpression binary)
        {
            foreach (var left in GetParameterExpressions(binary.Left))
                yield return left;

            foreach (var right in GetParameterExpressions(binary.Right))
                yield return right;

            yield break;
        }

        if (expression is ParameterExpression parameter)
            yield return parameter;

        if (expression is MemberExpression member && member.Expression is ParameterExpression p)
            yield return p;
    }

    public static object GetValue(MemberExpression member)
    {
        member.ThrowIfNull();

        var objectMember = Expression.Convert(member, typeof(object));
        var getterLambda = Expression.Lambda<Func<object>>(objectMember);
        var getter = getterLambda.Compile();

        return getter();
    }

    public static string NameOf(Expression<Func<object>> expression)
    {
        expression.ThrowIfNull();

        var mi = GetMemberInfo(expression);
        if (null == mi)
            throw new ArgumentOutOfRangeException(nameof(expression), "expression is not a member access");

        return mi.Name;
    }

    public static IEnumerable<Expression> Sort(this IEnumerable<Expression> expressions)
    {
        return expressions.OrderBy(x => x.GetExpressionHashCode());
    }
}
