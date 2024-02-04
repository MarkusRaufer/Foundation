using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

public static class ParameterHelper
{
    /// <summary>
    /// Returns true if all parameters are of same type and have the same name.
    /// </summary>
    /// <param name="expressions"></param>
    /// <returns></returns>
    public static bool AllEqual(IEnumerable<ParameterExpression> expressions)
    {
        var it = expressions.GetEnumerator();
        if (!it.MoveNext()) return true;

        var first = it.Current;
        while(it.MoveNext())
        {
            if (it.Current.Type != first.Type) return false;

            if(it.Current.Name != first.Name) return false;
        }
        
        return true;
    }
}
