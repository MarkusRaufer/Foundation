using Foundation.Linq.Expressions;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public class PropertyInfoCash : MemberCash<PropertyInfo>
{
    public PropertyInfoCash(Type memberType) : base(memberType)
    {
    }

    public PropertyInfoCash(Type memberType, PropertyInfo[] members) : base(memberType, members)
    {
    }

    public PropertyInfoCash(Type memberType, string[] memberNames) : base(memberType, memberNames)
    {
    }

    protected override PropertyInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.GetMember();
        if (null == member) throw new InvalidArgumentException($"{lambda.Body}");

        return member.Member is PropertyInfo pi
            ? pi
            : throw new InvalidArgumentException($"{member.Member}");
    }

    protected override IEnumerable<PropertyInfo> GetTypeMembers()
    {
        return MemberType.GetProperties();
    }
}

public class PropertyInfoCash<T> : MemberCash<T, PropertyInfo>
{
    public PropertyInfoCash() : base()
    {
    }

    public PropertyInfoCash(PropertyInfo[] members) : base(members)
    {
    }
    
    public PropertyInfoCash(string[] memberNames) : base(memberNames)
    {
    }

    public PropertyInfoCash(params Expression<Func<T, object>>[] members) : base(members)
    {
    }

    protected override PropertyInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.GetMember();
        if (null == member) throw new InvalidArgumentException($"{lambda.Body}");

        return member.Member is PropertyInfo pi 
            ? pi 
            : throw new InvalidArgumentException($"{member.Member}");
    }

    protected override IEnumerable<PropertyInfo> GetTypeMembers()
    {
        return typeof(T).GetProperties();
    }
}
