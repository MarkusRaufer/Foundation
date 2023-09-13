using Foundation.Linq.Expressions;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public class PropertyInfoCash : MemberCash<PropertyInfo>
{
    public PropertyInfoCash(Type type) : base(type)
    {
    }

    public PropertyInfoCash(Type type, PropertyInfo[] members) : base(type, members)
    {
    }

    public PropertyInfoCash(Type type, string[] memberNames) : base(type, memberNames)
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

    protected override IEnumerable<PropertyInfo> GetTypeMembers() => Type.GetProperties();
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

    protected override IEnumerable<PropertyInfo> GetTypeMembers() => typeof(T).GetProperties();
}
