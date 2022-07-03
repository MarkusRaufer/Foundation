using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Foundation.IO
{
    [TestFixture]
    public class BinaryObjectWriterTests
    {
        record Person(string Name, DateOnly Birthday, DateTime Created, TimeOnly Time);

        [Test]
        public void Serialize_Should_WritePerson_When_DefaultCtor_And_ObjectIsPerson()
        {
            var writer = new BinaryObjectWriter<Person>();

            var person1 = new Person("John", new(1972, 9, 11), new(2010, 1, 1), new(8, 30, 15, 9));
            var person2 = new Person("Alice", new(1989, 4, 1), new(2020, 11, 5), new(11, 22, 33, 44));

            using var stream = new MemoryStream();

            writer.WriteObject(stream, person1);
            writer.WriteObject(stream, person2);
            writer.WriteObject(stream, person1);

            using var reader = new BinaryReader(stream);
            stream.Position = 0;

            DateTime readDateTime()
            {
                var ticks = reader.ReadInt64();
                return new DateTime(ticks);
            }

            Person readPerson()
            {
                var name = reader.ReadString();
                var birthday = DateOnly.FromDateTime(readDateTime());
                var created = readDateTime();
                var time = new TimeOnly(reader.ReadInt64());

                return new Person(name, birthday, created, time);
            }

            Assert.AreEqual(person1, readPerson());
            Assert.AreEqual(person2, readPerson());
            Assert.AreEqual(person1, readPerson());
        }

        [Test]
        public void Serialize_Should_WritePropertiesNameAndCreated_When_CtorParamsAreNameAndCreated_And_ObjectIsPerson()
        {

            var properties = new[] { nameof(Person.Name), nameof(Person.Created) };
            var writer = new BinaryObjectWriter<Person>(properties);

            var person1 = new Person("John", new(1972, 9, 11), new(2010, 1, 1), new(8, 30, 15, 9));
            var person2 = new Person("Alice", new(1989, 4, 1), new(2020, 11, 5), new(11, 22, 33, 44));

            using var stream = new MemoryStream();

            writer.WriteObject(stream, person1);
            writer.WriteObject(stream, person2);
            writer.WriteObject(stream, person1);

            using var reader = new BinaryReader(stream);
            stream.Position = 0;

            DateTime readDateTime()
            {
                var ticks = reader.ReadInt64();
                return new DateTime(ticks);
            }

            IEnumerable<object> readPersonProperties()
            {
                yield return reader.ReadString();
                yield return readDateTime();
            }

            var props1 = new object[] { "John", new DateTime(2010, 1, 1) };
            var props2 = new object[] { "Alice", new DateTime(2020, 11, 5) };

            CollectionAssert.AreEqual(props1, readPersonProperties().ToArray());
            CollectionAssert.AreEqual(props2, readPersonProperties().ToArray());
            CollectionAssert.AreEqual(props1, readPersonProperties().ToArray());
        }
    }
}
