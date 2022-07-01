using Foundation.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public class MemberInfoCash<T>
{
    private readonly string[]? _memberNames;
    private MemberInfo[]? _members;

    public MemberInfoCash()
    {
    }

    public MemberInfoCash(IEnumerable<MemberInfo> members)
    {
        _members = members.ToArray().ThrowIfEmpty();
    }

    public MemberInfoCash(params string[] memberNames)
    {
        _memberNames = memberNames.ThrowIfEmpty();
    }

    public MemberInfoCash(params Expression<Func<T, object>>[] members)
    {
        if (0 == members.Length) throw new ArgumentOutOfRangeException(nameof(members));

        _members = members.Cast<LambdaExpression>()
                          .Select(GetMemberFromLambda)
                          .ToArray();
    }

    private static MemberInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.Body switch
        {
            MemberExpression me => me,
            UnaryExpression ue => (MemberExpression)ue.Operand,
            _ => throw new InvalidArgumentException($"{lambda.Body}")
        };

        return member.Member switch
        {
            FieldInfo => member.Member,
            PropertyInfo => member.Member,
            _ => throw new InvalidArgumentException($"{member.Member}")
        };
    }

    public IEnumerable<MemberInfo> GetMembers()
    {
        if (null != _members) return _members;

        var members = typeof(T).GetMembers();

        _members = null == _memberNames
            ? members
            : members.Where(member => _memberNames.Contains(member.Name)).ToArray();

        return _members;
    }
}
