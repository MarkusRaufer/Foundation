using Foundation.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.IO
{
    public class BinarySerializer
    {
        private readonly ICollection<string>? _memberNames;
        private ICollection<MemberInfo>? _memberCache;
        private readonly Type _type;

        public BinarySerializer(Type type)
        {
            _type = type.ThrowIfNull();
        }

        public BinarySerializer(Type type, IEnumerable<string> memberNames) : this(type)
        {
            _memberNames = memberNames.ThrowIfEmpty().ToArray();
        }

        public void Deserialze(object obj, Stream stream)
        {
            if (null == obj) throw new ArgumentNullException(nameof(obj));
            if (null == stream) throw new ArgumentNullException(nameof(stream));

            if(obj.GetType() != _type) 
                throw new InvalidArgumentException($"{nameof(obj)} is not of type {_type.FullName}");

            stream.Position = 0L;
            var reader = new BinaryReader(stream);
            reader.ReadToObject(obj, GetMembers());
        }

        private static IEnumerable<MemberInfo> FilterFieldsAndProperties(IEnumerable<MemberInfo> memberInfos)
        {
            return memberInfos.Where(mi => mi is FieldInfo || mi is PropertyInfo);
        }

        private IEnumerable<MemberInfo> GetMembers()
        {
            if (null != _memberCache) return _memberCache;

            var members = FilterFieldsAndProperties(_type.GetMembers());

            if(null != _memberNames && 0 < _memberNames.Count)
            {
                members = members.Where(member => _memberNames.Contains(member.Name));
            }

            _memberCache = members.ToArray();
            return _memberCache;
        }

        public static BinarySerializer New<T>(Type type, params Expression<Func<T, object>>[] members)
        {
            if(0 == members.Length) throw new ArgumentOutOfRangeException(nameof(members));

            var memberInfos = members.FilterMap(m => Opt.Maybe(m.Body as MemberExpression))
                                     .FilterMap(me =>
                                     {
                                         return me.Member switch
                                         {
                                             FieldInfo => Opt.Some(me.Member.Name),
                                             PropertyInfo => Opt.Some(me.Member.Name),
                                             _ => Opt.None<string>()
                                         };
                                     });
            return new BinarySerializer(type, memberInfos);
        }

        public void Serialze(object obj, Stream stream)
        {
            if (null == obj) throw new ArgumentNullException(nameof(obj));
            if (null == stream) throw new ArgumentNullException(nameof(stream));

            var writer = new BinaryWriter(stream);
            writer.WriteObject(obj, GetMembers());
        }
    }
}
