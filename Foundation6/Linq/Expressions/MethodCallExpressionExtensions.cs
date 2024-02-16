using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Linq.Expressions;

public static class MethodCallExpressionExtensions
{
    public static IEnumerable<ParameterExpression> GetParameters(this MethodCallExpression? expression)
    {
        if (expression == null) return Enumerable.Empty<ParameterExpression>();

        return expression.Arguments.SelectMany(x => x.GetParameters());
    }
}
