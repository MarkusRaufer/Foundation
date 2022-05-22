using Foundation.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Foundation.Reflection
{
    public static class ReflectionHelper
    {
        public static FieldInfo? GetBackingField([DisallowNull] PropertyInfo property)
        {
            if (property.DeclaringType is null) return null;

            return GetBackingField(property.DeclaringType, property.Name);
        }

        public static FieldInfo? GetBackingField<T>(string propertyName) => GetBackingField(typeof(T), propertyName);

        public static FieldInfo? GetBackingField([DisallowNull] this Type type, [DisallowNull] string propertyName)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            return type.GetField($"<{propertyName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static IEnumerable<MemberInfo> GetMembers(this Type type, IEnumerable<string> keys)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));

            return GetMembers(type,
                              BindingFlags.Public
                            | BindingFlags.NonPublic
                            | BindingFlags.Instance
                            | BindingFlags.FlattenHierarchy,
                              keys);
        }

        public static IEnumerable<MemberInfo> GetMembers(this Type type, BindingFlags bindingFlags, IEnumerable<string> keys)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));

            return keys.IntersectBy(type.GetMembers(bindingFlags),
                                    key => key,
                                    mi => mi.Name,
                                    (key, mi) => mi);
        }



        public static object? GetValue(object obj, MemberInfo memberInfo)
        {
            return memberInfo switch
            {
                FieldInfo fi => fi.GetValue(obj),
                PropertyInfo pi => pi.GetValue(obj),
                _ => throw new NotImplementedException(nameof(memberInfo))
            };
        }

        public static IEnumerable<KeyValuePair<MemberInfo, object?>> GetValues(object obj, params MemberInfo[] memberInfos)
        {
            foreach (var memberInfo in memberInfos)
            {
                var value = GetValue(obj, memberInfo);
                yield return Pair.New(memberInfo, value);
            }
        }

        public static object SetValue(object obj, string memberName, object value)
        {
            if (null == obj) throw new ArgumentNullException(nameof(obj));

            return SetValue(obj.GetType(), obj, memberName, value);
        }

        public static object SetValue(Type type, object obj, string memberName, object value)
        {
            return SetValue(type, 
                            obj, 
                            memberName, 
                            BindingFlags.Public
                          | BindingFlags.NonPublic
                          | BindingFlags.Instance
                          | BindingFlags.FlattenHierarchy, value);
        }

        public static object SetValue(Type type, object obj, string memberName, BindingFlags bindingFlags, object value)
        {
            obj.ThrowIfNull();
            memberName.ThrowIfNullOrWhiteSpace();

            var members = type.GetMember(memberName, bindingFlags);

            members.ThrowIfOutOfRange(() => 1 != memberName.Length);

            if (1 != members.Length) throw new ArgumentOutOfRangeException(nameof(memberName));

            return SetValue(type, obj, members[0], value);
        }

        public static object SetValue([DisallowNull] object obj, MemberInfo memberInfo, object value)
        {
            if (null == obj) throw new ArgumentNullException(nameof(obj));

            return SetValue(obj.GetType(), obj, memberInfo, value);
        }

        public static object SetValue([DisallowNull] Type type, [DisallowNull] object obj, [DisallowNull] MemberInfo memberInfo, object value)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));
            if (null == obj) throw new ArgumentNullException(nameof(obj));
            if (null == memberInfo) throw new ArgumentNullException(nameof(memberInfo));

            switch (memberInfo)
            {
                case FieldInfo fi:
                    fi.SetValue(obj, value);
                    break;
                case PropertyInfo pi:
                    if (pi.CanWrite)
                    {
                        pi.SetValue(obj, value);
                        break;
                    }
                        
                    var backingField = GetBackingField(type, pi.Name);
                    backingField?.SetValue(obj, value);
                    break;
            }

            return obj;
        }
    }
}
