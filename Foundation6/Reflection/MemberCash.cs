using Foundation.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public abstract class MemberCash<TInfo> : MemberCashBase<TInfo>
    where TInfo : MemberInfo
{
    public MemberCash(Type memberType)
    {
        MemberType = memberType.ThrowIfNull();
    }

    public MemberCash(Type memberType, TInfo[] members) : this(memberType)
    {
        Members = members.ThrowIfEmpty();
    }

    public MemberCash(Type memberType, string[] memberNames) : this(memberType)
    {
        MemberNames = memberNames.ThrowIfEmpty();
    }

    public Type MemberType { get; }
}

public abstract class MemberCash<T, TInfo> : MemberCashBase<TInfo>
    where TInfo : MemberInfo
{
    public MemberCash()
    {
    }

    public MemberCash(TInfo[] members)
    {
        Members = members.ThrowIfEmpty();
    }

    public MemberCash(string[] memberNames)
    {
        MemberNames = memberNames.ThrowIfEmpty();
    }

    public MemberCash(params Expression<Func<T, object>>[] members)
    {
        if (0 == members.Length) throw new ArgumentOutOfRangeException(nameof(members));

        Members = members.Cast<LambdaExpression>()
                         .Select(GetMemberFromLambda)
                         .Where(MemberFilter)
                         .ToArray();
    }
}
