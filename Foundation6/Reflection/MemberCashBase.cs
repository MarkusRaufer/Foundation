using Foundation.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public abstract class MemberCashBase<TInfo> where TInfo : MemberInfo
{
    protected abstract TInfo GetMemberFromLambda(LambdaExpression lambda);

    public IEnumerable<TInfo> GetMembers()
    {
        if (null != Members) return Members;

        var members = GetTypeMembers();
        if (null == MemberNames)
        {
            Members = null == MemberFilter
                ? members.ToArray()
                : members.Where(MemberFilter).ToArray();

            return Members;
        }
        Members = members.Where(member => MemberNames.Contains(member.Name)).ToArray();

        return Members;
    }

    /// <summary>
    /// Return type of meta information you want. Like PropertyInfo, FieldInfo,...
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<TInfo> GetTypeMembers();

    protected Func<TInfo, bool> MemberFilter { get; set; } = _ => true;

    protected string[]? MemberNames { get; set; }

    protected TInfo[]? Members { get; set; }
}
