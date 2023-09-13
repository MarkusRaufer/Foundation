using Foundation.Linq.Expressions;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public class MemberInfoCash : MemberCash<MemberInfo>
{
    public MemberInfoCash(Type type) : base(type, type.GetProperties())
    {
    }

    public MemberInfoCash(Type type, MemberInfo[] members) : base(type, members)
    {
    }

    public MemberInfoCash(Type type, string[] memberNames) : base(type, memberNames)
    {
    }

    protected override MemberInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.GetMember();
        if (null == member) throw new InvalidArgumentException($"{lambda}");

        return member.Member;
    }

    protected override IEnumerable<MemberInfo> GetTypeMembers() => Type.GetMembers();
}

public class MemberInfoCash<T> : MemberCash<T, MemberInfo>
{
    public MemberInfoCash() : base()
    {
    }

    public MemberInfoCash(MemberInfo[] members) : base(members)
    {
    }

    public MemberInfoCash(string[] memberNames) : base(memberNames)
    {
    }

    public MemberInfoCash(params Expression<Func<T, object>>[] members) : base(members)
    {
    }

    protected override MemberInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.GetMember();
        if(null == member) throw new InvalidArgumentException($"{lambda}");

        return member.Member;
    }

    protected override IEnumerable<MemberInfo> GetTypeMembers() => typeof(T).GetMembers();
}

