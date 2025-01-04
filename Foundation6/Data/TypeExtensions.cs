using System.Data;

namespace Foundation.Data;

public static class TypeExtensions
{
    public static DbType ToDbType(this Type type)
    {
        return type switch
        {
            Type t when t == typeof(bool) => DbType.Boolean,
            Type t when t == typeof(byte) => DbType.Byte,
            Type t when t == typeof(sbyte) => DbType.SByte,
            Type t when t == typeof(DateOnly) => DbType.Date,
            Type t when t == typeof(DateTime) => DbType.DateTime,
            Type t when t == typeof(DateTimeOffset) => DbType.DateTimeOffset,
            Type t when t == typeof(decimal) => DbType.Decimal,
            Type t when t == typeof(double) => DbType.Double,
            Type t when t == typeof(Guid) => DbType.Guid,
            Type t when t == typeof(Int16) => DbType.Int16,
            Type t when t == typeof(Int32) => DbType.Int32,
            Type t when t == typeof(Int64) => DbType.Int64,
            Type t when t == typeof(Single) => DbType.Single,
            Type t when t == typeof(string) => DbType.String,
            Type t when t == typeof(TimeOnly) => DbType.Time,
            Type t when t == typeof(UInt16) => DbType.UInt16,
            Type t when t == typeof(UInt32) => DbType.UInt32,
            Type t when t == typeof(UInt64) => DbType.UInt64,
            _ => DbType.Binary,
        };
    }
}
