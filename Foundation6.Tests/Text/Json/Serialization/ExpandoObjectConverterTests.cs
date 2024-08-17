using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Dynamic;
using FluentAssertions;


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
        json.Should().Be(expected);
    }
}
