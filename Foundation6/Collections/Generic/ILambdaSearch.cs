using System.Linq.Expressions;

namespace Foundation.Collections.Generic;

public interface ILambdaSearch<T, TResult>
{
    TResult FindAll(LambdaExpression lambda);
}
