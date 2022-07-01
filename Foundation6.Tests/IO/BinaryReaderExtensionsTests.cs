using NUnit.Framework;
using System;
using System.IO;

namespace Foundation.IO
{
    [TestFixture]
    public class BinaryReaderExtensionsTests
    {
        [Test]
        public void ReadDateOnly_Should_ReturnsDateOnly_When_ExistsInStream()
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

            Assert.AreEqual(date1, reader.ReadDateOnly());
            Assert.AreEqual(date2, reader.ReadDateOnly());
            Assert.AreEqual(date1, reader.ReadDateOnly());
        }

        [Test]
        public void ReadDateTime_Should_ReturnsDateTime_When_ExistsInStream()
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

            Assert.AreEqual(dt1, reader.ReadDateTime());
            Assert.AreEqual(dt2, reader.ReadDateTime());
            Assert.AreEqual(dt1, reader.ReadDateTime());
        }

        [Test]
        public void ReadGuid_Should_ReturnsGuid_When_ExistsInStream()
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

            Assert.AreEqual(guid1, reader.ReadGuid());
            Assert.AreEqual(guid2, reader.ReadGuid());
            Assert.AreEqual(guid1, reader.ReadGuid());
        }

        [Test]
        public void ReadTimeOnly_Should_ReturnsTimeOnly_When_ExistsInStream()
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

            Assert.AreEqual(time1, reader.ReadTimeOnly());
            Assert.AreEqual(time2, reader.ReadTimeOnly());
            Assert.AreEqual(time1, reader.ReadTimeOnly());
        }
    }
}
