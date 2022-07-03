using Foundation.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection;

public abstract class MemberCash<T, TInfo> where TInfo : MemberInfo
{
    private readonly string[]? _memberNames;
    private TInfo[]? _members;

    public MemberCash()
    {
    }

    public MemberCash(TInfo[] members)
    {
        _members = members.ThrowIfEmpty();
    }

    public MemberCash(string[] memberNames)
    {
        _memberNames = memberNames.ThrowIfEmpty();
    }

    public MemberCash(params Expression<Func<T, object>>[] members)
    {
        if (0 == members.Length) throw new ArgumentOutOfRangeException(nameof(members));

        _members = members.Cast<LambdaExpression>()
                          .Select(GetMemberFromLambda)
                          .Where(MemberFilter)
                          .ToArray();
    }

    protected abstract TInfo GetMemberFromLambda(LambdaExpression lambda);

    public IEnumerable<TInfo> GetMembers()
    {
        if (null != _members) return _members;

        var members = GetTypeMembers();
        if(null == _memberNames)
        {
            _members = null == MemberFilter 
                ? members.ToArray() 
                : members.Where(MemberFilter).ToArray();

            return _members;
        }
        _members = members.Where(member => _memberNames.Contains(member.Name)).ToArray();

        return _members;
    }

    protected abstract IEnumerable<TInfo> GetTypeMembers();

    protected Func<TInfo, bool> MemberFilter { get; set; } = _ => true;

}
