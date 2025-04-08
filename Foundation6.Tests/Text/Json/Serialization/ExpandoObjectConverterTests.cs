using NUnit.Framework;
using Shouldly;
using System.Dynamic;
using System.Text.Json;


namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class ExpandoObjectConverterTests
{
    [Test]
    public void Test()
    {
        dynamic obj = new ExpandoObject();
        obj.FirstName = "Markus";
        obj.IsDirty = true;
        obj.Alternatives = new string[] { "1", "2", "3" };
        obj.State = new ExpandoObject();
        obj.State.Name = "myName";
        obj.State.Description = "my description";

        var options = new JsonSerializerOptions
        {
            Converters = { new ExpandoObjectJsonConverter() }
        };

        var json = JsonSerializer.Serialize<ExpandoObject>(obj, options) as string;

        var expected = """{"FirstName":"Markus","IsDirty":true,"Alternatives":["1","2","3"],"State":{"Name":"myName","Description":"my description"}}""";
        json.ShouldBe(expected);
    }
}
