using Foundation.Linq.Expressions;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

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

    protected override IEnumerable<MemberInfo> GetTypeMembers()
    {
        return typeof(T).GetMembers();
    }
}

