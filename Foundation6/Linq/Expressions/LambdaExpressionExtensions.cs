using System.Linq.Expressions;

namespace Foundation.Linq.Expressions
{
    public static class LambdaExpressionExtensions
    {
        public static MemberExpression? GetMember(this LambdaExpression lambda)
        {
            return lambda.Body switch
            {
                MemberExpression me => me,
                UnaryExpression ue => ue.Operand as MemberExpression,
                _ => null,
            };
        }
    }
}
