using Foundation.Collections.Generic;
using Foundation.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.IO
{
    public class BinarySerializer
    {
        private readonly ICollection<string> _memberNames;
        private ICollection<MemberInfo> _members;

        public BinarySerializer()
        {
        }

        public BinarySerializer(IEnumerable<MemberInfo> members)
        {
            _members = FilterMembers(members.ThrowIfEmpty(nameof(members))).ToArray();
        }

        public BinarySerializer(IEnumerable<string> memberNames)
        {
            _memberNames = memberNames.ThrowIfEmpty(nameof(memberNames)).ToArray();
        }

        public void Deserialze(object obj, Stream stream)
        {
            if (null == obj) throw new ArgumentNullException(nameof(obj));
            if (null == stream) throw new ArgumentNullException(nameof(stream));

            stream.Position = 0L;
            var reader = new BinaryReader(stream);
            reader.ReadObject(obj, GetMembers(obj.GetType()));
        }

        private static IEnumerable<MemberInfo> FilterMembers(IEnumerable<MemberInfo> memberInfos)
        {
            return memberInfos.Where(mi => mi is FieldInfo || mi is PropertyInfo);
        }

        private IEnumerable<MemberInfo> GetMembers(Type type)
        {
            if (null != _members && 0 < _members.Count) return _members;
            
            if(null != _memberNames && 0 < _memberNames.Count)
            {
                _members = FilterMembers(type.GetMembers(_memberNames)).ToArray();
                return _members;
            }

            _members = FilterMembers(type.GetMembers()).ToArray();
            return _members;
        }

        public static BinarySerializer New<T>(params Expression<Func<T, object>>[] members)
        {
            if(0 == members.Length) throw new ArgumentOutOfRangeException(nameof(members));

            var memberInfos = members.FilterMap(m => Opt.Maybe(m.Body as MemberExpression))
                                     .FilterMap(me =>
                                     {
                                         return me.Member switch
                                         {
                                             FieldInfo => Opt.Some(me.Member),
                                             PropertyInfo => Opt.Some(me.Member),
                                             _ => Opt.None<MemberInfo>()
                                         };
                                     });
            return new BinarySerializer(memberInfos);
        }

        public void Serialze(object obj, Stream stream)
        {
            if (null == obj) throw new ArgumentNullException(nameof(obj));
            if (null == stream) throw new ArgumentNullException(nameof(stream));

            var writer = new BinaryWriter(stream);
            writer.WriteObject(obj, GetMembers(obj.GetType()));
        }
    }
}
