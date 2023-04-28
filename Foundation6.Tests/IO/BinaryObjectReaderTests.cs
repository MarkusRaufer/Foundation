using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.IO
{
    [TestFixture]
    public class BinaryObjectReaderTests
    {
        record Person(string Name, DateOnly Birthday, DateTime Created, TimeOnly Time);

        [Test]
        public void Serialize_Should_WritePerson_When_DefaultCtor_And_ObjectIsPerson()
        {

            var person1 = new Person("John", new(1972, 9, 11), new(2010, 1, 1), new(8, 30, 15, 9));
            var person2 = new Person("Alice", new(1989, 4, 1), new(2020, 11, 5), new(11, 22, 33, 44));

            using var stream = new MemoryStream();

            var writer = new BinaryObjectWriter<Person>();

            writer.WriteObject(stream, person1);
            writer.WriteObject(stream, person2);
            writer.WriteObject(stream, person1);

            var reader = new BinaryObjectReader<Person>();
            stream.Position = 0;

            static IEnumerable<KeyValuePair<string, object>> properties(Person person)
            {
                yield return new KeyValuePair<string, object>(nameof(Person.Birthday), person.Birthday);
                yield return new KeyValuePair<string, object>(nameof(Person.Created), person.Created);
                yield return new KeyValuePair<string, object>(nameof(Person.Name), person.Name);
                yield return new KeyValuePair<string, object>(nameof(Person.Time), person.Time);
            }

            {
                var keyValues = reader.ReadKeyValues(stream).ToArray();
                Assert.IsTrue(properties(person1).EqualsCollection(keyValues));
            }
            {
                var keyValues = reader.ReadKeyValues(stream).ToArray();
                Assert.IsTrue(properties(person2).EqualsCollection(keyValues));
            }
            {
                var keyValues = reader.ReadKeyValues(stream).ToArray();
                Assert.IsTrue(properties(person1).EqualsCollection(keyValues));
            }
        }
    }
}
