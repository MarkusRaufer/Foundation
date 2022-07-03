using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Foundation.IO
{
    [TestFixture]
    public class BinaryWriterExtensionsTests
    {
        record Person(string Name, DateOnly Birthday, DateTime Created, TimeOnly Time);

        [Test]
        public void Write_Should_WriteDateOnly_When_ValueIsDateOnly()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var date1 = new DateOnly(2010, 3, 9);
            var date2 = new DateOnly(2014, 7, 27);

            writer.Write(date1);
            writer.Write(date2);
            writer.Write(date1);

            using var reader = new BinaryReader(stream);
            stream.Position = 0;

            DateOnly readDate()
            {
                var ticks = reader.ReadInt64();
                var dt = new DateTime(ticks);
                return DateOnly.FromDateTime(dt);
            }

            Assert.AreEqual(date1, readDate());
            Assert.AreEqual(date2, readDate());
            Assert.AreEqual(date1, readDate());
        }

        [Test]
        public void Write_Should_WriteDateTime_When_ValueIsDateTime()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var dt1 = new DateTime(2010, 3, 9, 8, 30, 15);
            var dt2 = new DateTime(2014, 7, 27, 9, 45, 30);

            writer.Write(dt1);
            writer.Write(dt2);
            writer.Write(dt1);

            using var reader = new BinaryReader(stream);
            stream.Position = 0;

            DateTime readDateTime()
            {
                var ticks = reader.ReadInt64();
                return new DateTime(ticks);
            }

            Assert.AreEqual(dt1, readDateTime());
            Assert.AreEqual(dt2, readDateTime());
            Assert.AreEqual(dt1, readDateTime());
        }

        [Test]
        public void Write_Should_WriteGuid_When_ValueIsGuid()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();

            writer.Write(guid1);
            writer.Write(guid2);
            writer.Write(guid1);

            using var reader = new BinaryReader(stream);
            stream.Position = 0;

            Guid readGuid()
            {
                var bytes = reader.ReadBytes(16);
                return new Guid(bytes);
            }

            Assert.AreEqual(guid1, readGuid());
            Assert.AreEqual(guid2, readGuid());
            Assert.AreEqual(guid1, readGuid());
        }

        [Test]
        public void Write_Should_WriteTimeOnly_When_ValueIsTimeOnly()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var time1 = new TimeOnly(8, 9, 10, 11);
            var time2 = new TimeOnly(11, 22, 33, 44);

            writer.Write(time1);
            writer.Write(time2);
            writer.Write(time1);

            using var reader = new BinaryReader(stream);
            stream.Position = 0;

            TimeOnly readTime()
            {
                var ticks = reader.ReadInt64();
                return new TimeOnly(ticks);
            }

            Assert.AreEqual(time1, readTime());
            Assert.AreEqual(time2, readTime());
            Assert.AreEqual(time1, readTime());
        }

        [Test]
        public void WriteObject_Should_WritePerson_When_ObjectIsPerson()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var person1 = new Person("John", new(1972, 9, 11), new(2010, 1, 1), new(8, 30, 15, 9));
            var person2 = new Person("Alice", new(1989, 4, 1), new(2020, 11, 5), new(11, 22, 33, 44));

            writer.WriteObject(person1);
            writer.WriteObject(person2);
            writer.WriteObject(person1);

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

        public void WriteObject_Should_WritePropertiesNameAndCreated_When_ObjectIsPerson()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var person1 = new Person("John", new(1972, 9, 11), new(2010, 1, 1), new(8, 30, 15, 9));
            var person2 = new Person("Alice", new(1989, 4, 1), new(2020, 11, 5), new(11, 22, 33, 44));

            var properties = new[] { "Name", "Created" };
            writer.WriteObject(person1, properties);
            writer.WriteObject(person2, properties);
            writer.WriteObject(person1, properties);

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
