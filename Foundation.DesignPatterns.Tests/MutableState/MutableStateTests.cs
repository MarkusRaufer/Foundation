using FluentAssertions;
using Foundation.Collections.Generic;
using Foundation.DesignPatterns.Tests.MutableState.Commands;

namespace Foundation.DesignPatterns.MutableState
{
    public class MutableStateTests
    {
        [Fact]
        public void ApplyEvent_ShouldReturnPropertiesReplacedEvent_When_UsedReplacePropertiesCommand()
        {
            var properties = new Dictionary<string, object?>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" },
                { "Birthday", new DateTime(1970, 4, 1) },
            };

            var id = Id.New();
            var objectType = "Person";

            var sut = new PropertiesState(new DictionaryValue<string, object?>(properties));
            var entity = new Entity(objectType, id, sut);

            var newLastName = "Miller";
            var replaceProperties = new Dictionary<string, object?> { { "LastName", newLastName } };
            var ev = sut.HandleCommand(new ReplaceProperties(objectType, id, new DictionaryValue<string, object?>(replaceProperties)));
            sut.ApplyEvent(ev);

            var lastName = entity.Properties["LastName"];
            lastName.Should().Be(newLastName);
        }

        [Fact]
        public void HandleCommand_ShouldReturnPropertiesReplacedEvent_When_UsedReplacePropertiesCommand()
        {
            var properties = new Dictionary<string, object?>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" },
                { "Birthday", new DateTime(1970, 4, 1) },
            };

            var id = Id.New();
            var objectType = "Person";

            var sut = new PropertiesState(new DictionaryValue<string, object?>(properties));
            var entity = new Entity(objectType, id, sut);

            var newLastName = "Miller";
            var replaceProperties = new Dictionary<string, object?> { { "LastName", newLastName } };
            var ev = sut.HandleCommand(new ReplaceProperties(objectType, id, new DictionaryValue<string, object?>(replaceProperties)));
            sut.ApplyEvent(ev);

            ev.Properties.Count.Should().Be(1);
            entity.Properties["LastName"].Should().Be(newLastName);
        }
    }
}