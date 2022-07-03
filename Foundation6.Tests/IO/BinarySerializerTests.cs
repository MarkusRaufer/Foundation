using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Foundation.IO
{
    [TestFixture]
    public class BinarySerializerTests
    {
        record Person(string FirstName, string LastName, DateOnly Birthday);

        [Test]
        public void Serialize_Deserialze_Should_ReturnAPerson_WhenStreamHasData()
        {
            var expected = new Person("Peter", "Pan", new (1966, 4, 1));

            var sut = BinarySerializer.New(
                members =>
                {
                    var firstName = (string)members[nameof(Person.FirstName)];
                    var lastName =  (string)members[nameof(Person.LastName)];
                    var birthDay = (DateOnly)members[nameof(Person.Birthday)];
                    
                    return new Person(firstName, lastName, birthDay);
                },
                p => p.FirstName,
                p => p.LastName,
                p => p.Birthday);

            using var stream = new MemoryStream();

            sut.Serialize(expected, stream);

            stream.Position = 0;
            var person = sut.Deserialize(stream);

            Assert.IsNotNull(person);
            Assert.AreEqual(expected, person);
        }

        [Test]
        public void Serialize_DeserializeKeyValues_Should_ReturnADictionary_WhenStreamHasData()
        {
            var person = new Person("Peter", "Pan", new(1966, 4, 1));

            var sut = BinarySerializer.New(
                members =>
                {
                    var firstName = (string)members[nameof(Person.FirstName)];
                    var lastName = (string)members[nameof(Person.LastName)];
                    var birthDay = (DateOnly)members[nameof(Person.Birthday)];

                    return new Person(firstName, lastName, birthDay);
                },
                p => p.FirstName,
                p => p.LastName,
                p => p.Birthday);

            using var stream = new MemoryStream();

            sut.Serialize(person, stream);

            stream.Position = 0;
            var keyValues = sut.DeserializeKeyValues(stream);

            var expected = new Dictionary<string, object>
            {
                { nameof(Person.FirstName), person.FirstName },
                { nameof(Person.LastName), person.LastName },
                { nameof(Person.Birthday), person.Birthday },
            };

            Assert.IsNotNull(keyValues);
            CollectionAssert.AreEqual(expected, keyValues);
        }
    }
}
