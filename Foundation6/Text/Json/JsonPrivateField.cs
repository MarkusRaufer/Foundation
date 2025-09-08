using System.Text.Json.Serialization;

namespace Foundation.Text.Json;

/// <summary>
/// Indicates that the field should be included for serialization and deserialization.
/// </summary>
/// <remarks>
/// This can be applied to public and private fields.
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class JsonPrivateField : JsonAttribute
{
}
