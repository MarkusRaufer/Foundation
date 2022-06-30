using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.IO;

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
                    var firstName = (string)members.Nth(nameof(Person.FirstName)).OrThrow();
                    var lastName =  (string)members.Nth(nameof(Person.LastName)).OrThrow();
                    var birthDay = (DateOnly)members.Nth(nameof(Person.Birthday)).OrThrow();
                    
                    return new Person(firstName, lastName, birthDay);
                },
                p => p.FirstName,
                p => p.LastName,
                p => p.Birthday);

            using var stream = new MemoryStream();

            sut.Serialze(expected, stream);

            var person = sut.Deserialze<Person>(stream).OrThrow();

            Assert.AreEqual(expected, person);
        }
    }
}
