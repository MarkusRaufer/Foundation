using Foundation.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Foundation.Reflection
{
    public static class ReflectionHelper
    {
        public static FieldInfo? GetBackingField(PropertyInfo property)
        {
            if (property.DeclaringType is null) return null;

            return GetBackingField(property.DeclaringType, property.Name);
        }

        public static FieldInfo? GetBackingField<T>(string propertyName) => GetBackingField(typeof(T), propertyName);

        public static FieldInfo? GetBackingField(this Type type, string propertyName)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            return type.GetField($"<{propertyName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static IEnumerable<MemberInfo> GetMembers(this Type type, IEnumerable<string> memberNames)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));

            return GetMembers(type,
                              BindingFlags.Public
                            | BindingFlags.NonPublic
                            | BindingFlags.Instance
                            | BindingFlags.FlattenHierarchy,
                              memberNames);
        }

        public static IEnumerable<MemberInfo> GetMembers(this Type type, BindingFlags bindingFlags, IEnumerable<string> memberNames)
        {
            if (null == type) throw new ArgumentNullException(nameof(type));

            return memberNames.FilterMap(name => type.GetMember(name).FirstAsOpt());
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
            memberName.ThrowIfNullOrWhiteSpace();

            return SetValue(obj.ThrowIfNull(), 
                            memberName, 
                            BindingFlags.Public
                          | BindingFlags.NonPublic
                          | BindingFlags.Instance
                          | BindingFlags.FlattenHierarchy, value);
        }

        public static object SetValue(object obj, string memberName, BindingFlags bindingFlags, object value)
        {
            obj.ThrowIfNull();
            memberName.ThrowIfNullOrWhiteSpace();

            var members = obj.GetType().GetMember(memberName, bindingFlags);

            members.ThrowIfOutOfRange(() => 1 != memberName.Length);

            if (1 != members.Length) throw new ArgumentOutOfRangeException(nameof(memberName));

            return SetValue(obj, members[0], value);
        }

        public static object SetValue(object obj, MemberInfo memberInfo, object value)
        {
            if (null == obj) throw new ArgumentNullException(nameof(obj));
            if (null == memberInfo) throw new ArgumentNullException(nameof(memberInfo));

            switch (memberInfo)
            {
                case FieldInfo fi:
                    if(!fi.IsInitOnly) fi.SetValue(obj, value);
                    break;
                case PropertyInfo pi:
                    if (pi.CanWrite)
                    {
                        pi.SetValue(obj, value);
                        break;
                    }
                        
                    var backingField = GetBackingField(obj.GetType(), pi.Name);
                    backingField?.SetValue(obj, value);
                    break;
            }

            return obj;
        }
    }
}
