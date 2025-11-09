using FluentAssertions;
using Foundation.Text.Json.Serialization;
using NUnit.Framework;
using Shouldly;
using System;

namespace Foundation.ComponentModel;

[TestFixture]

public class MutableOwnerShipOwnerTests
{
    private record SetBuiltOn(Action<DateOnly> Delegate) : IHasDelegate<Action<DateOnly>>;
    private record SetCity(Action<string> Delegate) : IHasDelegate<Action<string>>;
    private record SetStreet(Action<string> Delegate) : IHasDelegate<Action<string>>;

    private class Address : IMutableOwnerShipOwner
    {
        private DateOnly _builtOn;
        private string _city;
        private string _street;
        private SetBuiltOn? _setFoundetOn;
        private SetCity? _setCity;
        private SetStreet? _setStreet;

        public Address(string city, string street, DateOnly builtOn)
        {
            _city = city;
            _street = street;
            _builtOn = builtOn;

            _setFoundetOn = new SetBuiltOn(setBuiltOn);
            _setCity = new SetCity(setCity);
            _setStreet = new SetStreet(setStreet);

            void setBuiltOn(DateOnly x) => _builtOn = x;
            void setCity(string x) => _city = x;
            void setStreet(string x) => _street = x;
        }

        public DateOnly BuiltOn => _builtOn;
        public string City => _city;
        public string Street => _street;

        public bool HasOwnerShip<T>()
        {
            return _setFoundetOn is T || _setCity is T || _setStreet is T;
        }

        public T? MoveOwnerShip<T>()
        {
            if (_setFoundetOn is T f)
            {
                _setFoundetOn = null;
                return f;
            }

            if (_setCity is T c)
            {
                _setCity = null;
                return c;
            }

            if (_setStreet is T s)
            {
                _setStreet = null;
                return s;
            }

            return default;
        }
    }

    [Test]
    public void Invoke_Should_CallDelegate_When_Using_IDelegateOwner()
    {
        //Arrange
        var owner = new Address("Berlin", "Marienstrasse", new DateOnly(1972, 3, 1));
        var setStreet = owner.MoveOwnerShip<SetStreet>();
        setStreet.ShouldNotBeNull();

        var changedStreet = "Hauptstrasse";

        // Act
        setStreet.Delegate(changedStreet);
        var owned2 = owner.MoveOwnerShip<SetStreet>();

        // Assert
        owner.Street.ShouldBe(changedStreet);
        owned2.ShouldBeNull();
    }
}
