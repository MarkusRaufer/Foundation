using Foundation.Collections.Generic;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions
{
    public static class LambdaExpressionExtensions
    {
        public static bool EqualsToExpression(this LambdaExpression lhs, LambdaExpression rhs, bool ignoreParameterName = true)
        {
            if (null == lhs) return null == rhs;

            Func<ParameterExpression, ParameterExpression, bool> equal = ignoreParameterName
            ? (ParameterExpression left, ParameterExpression right) => left.Type == right.Type
            : (ParameterExpression left, ParameterExpression right) => left.Name == right.Name && left.Type == right.Type;

            return null != rhs
                && lhs.NodeType == rhs.NodeType
                && lhs.Type == rhs.Type
                && lhs.ReturnType == rhs.ReturnType
                && lhs.Parameters.SequenceEqual(rhs.Parameters, (l, r) => equal(l, r))
                && lhs.Body.EqualsToExpression(rhs.Body);
        }

        public static int GetExpressionHashCode(this LambdaExpression expression)
        {
            expression.ThrowIfNull();

            var hashCodes = Enumerable.Empty<int>();
            hashCodes = hashCodes.Append(expression.NodeType.GetHashCode());
            hashCodes = hashCodes.Append(expression.Type.GetHashCode());
            hashCodes = hashCodes.Append(expression.ReturnType.GetHashCode());
            hashCodes = hashCodes.Concat(expression.Parameters.Select(x => x.GetExpressionHashCode(false)));
            hashCodes = hashCodes.Append(expression.Body.GetExpressionHashCode());

            return HashCode.FromOrderedHashCode(hashCodes.ToArray());
        }

        public static MemberExpression? GetMember(this LambdaExpression lambda)
        {
            return lambda.Body switch
            {
                MemberExpression me => me,
                UnaryExpression ue => ue.Operand as MemberExpression,
                _ => null,
            };
        }

        public static IEnumerable<Type> GetUniqueParameterTypes(this LambdaExpression lambda)
            => lambda.Parameters.Select(x => x.Type).Distinct();
        
        public static bool IsPredicate(this LambdaExpression lambda)
        {
            return lambda.ReturnType == typeof(bool);
        }
    }
}
