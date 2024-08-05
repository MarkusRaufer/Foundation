using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Foundation.Text.Json;

[TestFixture]
public class Utf8JsonReaderExtensionsTests
{
    [Test]
    public void Test()
    {
        var json =
            """
            {
                "first-name": "Peter",
                "last-name": "Pan",
                "age": 14,
                "weight": 56.78
            }
            """;

        var json2 = JsonNode.Parse(json);

        var dictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
    }
}
