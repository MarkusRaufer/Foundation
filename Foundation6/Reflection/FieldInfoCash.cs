using Foundation.Linq.Expressions;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.Reflection
{
    public class FieldInfoCash<T> : MemberCash<T, FieldInfo>
    {
        public FieldInfoCash() : base()
        {
        }

        public FieldInfoCash(FieldInfo[] members) : base(members)
        {
        }
        
        public FieldInfoCash(string[] memberNames) : base(memberNames)
        {
        }

 
        protected override FieldInfo GetMemberFromLambda(LambdaExpression lambda)
        {
            var member = lambda.GetMember();
            if (null == member) throw new InvalidArgumentException($"{lambda.Body}");

            return member.Member is FieldInfo pi 
                ? pi 
                : throw new InvalidArgumentException($"{member.Member}");
        }

        protected override IEnumerable<FieldInfo> GetTypeMembers()
        {
            return typeof(T).GetFields();
        }
    }
}
