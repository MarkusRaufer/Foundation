using Foundation.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.ComponentModel;

[TestFixture]
public class InterceptionExtensionsTests
{
    private class Person
    {
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public DateOnly Birthday { get; set; }
    }

    private record Pet(string Name, DateOnly Birthday);

    [Test]
    public void ChangeWith_Should_ThrowException_When_ClassWithImmutablePropertyIsUsed()
    {
        // Arrange
        var firstName = "Peter";
        var lastName = "Pan";
        var person = new Person(firstName, lastName) { Birthday = new DateOnly(1961, 2, 3) };
        IDictionary<string, object?> changes = null;

        var newBirthday = new DateOnly(1962, 3, 4);
        var newLastName = "Doe";

        // Act
        var ex = Should.Throw<ArgumentException>(() => person.ChangeWith(x => x.LastName, newLastName)
                       .And(x => x.Birthday, newBirthday)
                       .Build(x => changes = x));

        // Assert
        ex.ShouldNotBeNull();
    }

    [Test]
    public void ChangeWith_Should_ChangeProperty_When_ClassWithMutablePropertyIsUsed()
    {
        // Arrange
        var firstName = "Peter";
        var lastName = "Pan";
        var person = new Person(firstName, lastName) { Birthday = new DateOnly(1961, 2, 3) };
        IDictionary<string, object?> changes = null;

        var newBirthday = new DateOnly(1962, 3, 4);

        // Act
        var p2 = person.ChangeWith(x => x.Birthday, newBirthday)
                       .Build(x => changes = x);

        // Assert
        var eq = ReferenceEquals(person, p2);
        eq.ShouldBeTrue();

        p2.FirstName.ShouldBe(firstName);
        p2.LastName.ShouldBe(lastName);
        p2.Birthday.ShouldBe(newBirthday);

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(1);

        var change = changes.First();
        var expected = new KeyValuePair<string, object?>(nameof(Person.Birthday), newBirthday);

        change.ShouldBe(expected);
    }

    [Test]
    public void ChangeWith_Should_ChangeProperty_When_RecordWithReadOnlyPropertyIsUsed()
    {
        // Arrange
        var name = "Fips";
        var pet = new Pet(name, new DateOnly(2024, 5, 6));
        IDictionary<string, object?>? changes = default;
        var newBirthday = new DateOnly(2025, 6, 7);

        // Act
        var p2 = pet.ChangeWith(x => x.Birthday, newBirthday)
                    .Build(x => changes = x);

        // Assert
        var eq = ReferenceEquals(pet, p2);
        eq.ShouldBeTrue();

        p2.Name.ShouldBe(name);
        p2.Birthday.ShouldBe(newBirthday);

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(1);
        var change = changes.First();

        var expected = new KeyValuePair<string, object?>(nameof(Pet.Birthday), newBirthday);
        change.ShouldBe(expected);
    }

    [Test]
    public void ChangeWith_Should_ThrowException_When_ClassWithReadOnlyPropertyIsUsed()
    {
        // Arrange
        var person = new Person("Peter", "Pan") { Birthday = new DateOnly(1961, 2, 3) };
        IDictionary<string, object?>? changes = null;
        var newLastName = "Doe";

        // Act
        var ex = Should.Throw<ArgumentException>(() => person.ChangeWith(x => x.LastName, newLastName).Build(x => changes = x));

        // Assert
        ex.ShouldNotBeNull();
    }

    [Test]
    public void NewWith_Should_ReturnNewObjectWithChangedProperty_When_ClassWithReadOnlyPropertyIsUsed()
    {
        // Arrange
        var firstName = "Peter";
        var birthday = new DateOnly(1961, 2, 3);
        var person = new Person(firstName, "Pan") { Birthday = birthday };
        IDictionary<string, object?>? changes = null;
        var newLastName = "Doe";

        // Act
        var p2 = person.NewWith(x => x.LastName, "Doe")
                       .Build(x => changes = x);


        // Assert
        var eq = ReferenceEquals(person, p2);
        eq.ShouldBeFalse();

        p2.FirstName.ShouldBe(firstName);
        p2.LastName.ShouldBe(newLastName);
        p2.Birthday.ShouldBe(birthday);

        
        var expected = new KeyValuePair<string, object?>(nameof(Person.LastName), newLastName);
        //changes.ShouldBe(expected);
    }

    [Test]
    public void NewWith_Should_ReturnNewObjectWith2ChangedProperties_When_ClassWithReadOnlyPropertyIsUsed()
    {
        // Arrange
        var person = new Person("Peter", "Pan") { Birthday = new DateOnly(1961, 2, 3) };
        IDictionary<string, object?>? changes = null;
        var newFirstName = "John";
        var newLastName = "Doe";

        // Act
        //var p2 = person.NewWith(x => x.LastName, newLastName, x => changes = x);

        var p2 = person.NewWith(x => x.LastName, newLastName)
                       .And(x => x.FirstName, newFirstName)
                       .Build(x => changes = x);


        // Assert
        var eq = ReferenceEquals(person, p2);
        eq.ShouldBeFalse();

        p2.FirstName.ShouldBe(newFirstName);
        p2.LastName.ShouldBe(newLastName);

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);

        {
            var firstName = changes[nameof(Person.FirstName)] as string;
            firstName.ShouldBe(newFirstName);
        }
        {
            var lastName = changes[nameof(Person.LastName)] as string;
            lastName.ShouldBe(newLastName);
        }
    }

    [Test]
    public void NewWith_Should_ReturnNewObjectWith2ChangedProperties_When_ClassWithPropertiesIsUsed()
    {
        // Arrange
        var person = new Person("Peter", "Pan") { Birthday = new DateOnly(1961, 2, 3) };
        IDictionary<string, object?>? changes = null;
        var newFirstName = "John";
        var newLastName = "Doe";
        Dictionary<string, object?> properties = new()
        {
            { nameof(Person.FirstName), newFirstName  },
            { nameof(Person.LastName), newLastName }
        };

        // Act
        var p2 = person.NewWith(properties)
                       .Build(x => changes = x);


        // Assert
        var eq = ReferenceEquals(person, p2);
        eq.ShouldBeFalse();

        p2.FirstName.ShouldBe(newFirstName);
        p2.LastName.ShouldBe(newLastName);

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);

        {
            var firstName = changes[nameof(Person.FirstName)] as string;
            firstName.ShouldBe(newFirstName);
        }
        {
            var lastName = changes[nameof(Person.LastName)] as string;
            lastName.ShouldBe(newLastName);
        }
    }

    [Test]
    public void NewWith_Should_ReturnNewObjectWith3ChangedProperties_When_ClassWithPropertiesIsUsed()
    {
        // Arrange
        var person = new Person("Peter", "Pan") { Birthday = new DateOnly(1961, 2, 3) };
        IDictionary<string, object?>? changes = null;
        var newFirstName = "John";
        var newLastName = "Doe";
        var newBirthday = new DateOnly(1964, 5, 6);

        Dictionary<string, object?> properties = new()
        {
            { nameof(Person.FirstName), newFirstName  },
            { nameof(Person.LastName), newLastName }
        };

        // Act
        var p2 = person.NewWith(properties)
                       .And([nameof(Person.Birthday).ToKeyValue(newBirthday)])
                       .Build(x => changes = x);


        // Assert
        var eq = ReferenceEquals(person, p2);
        eq.ShouldBeFalse();

        p2.FirstName.ShouldBe(newFirstName);
        p2.LastName.ShouldBe(newLastName);
        p2.Birthday.ShouldBe(newBirthday);

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(3);

        {
            var firstName = changes[nameof(Person.FirstName)] as string;
            firstName.ShouldBe(newFirstName);
        }
        {
            var lastName = changes[nameof(Person.LastName)] as string;
            lastName.ShouldBe(newLastName);
        }
        {
            var birthday = changes[nameof(Person.Birthday)];
            birthday.ShouldBe(newBirthday);
        }
    }

    [Test]
    public void NewWith_Should_ReturnNewObjectWithChangedProperty_When_RecordWithReadOnlyPropertyIsUsed()
    {
        // Arrange
        var name = "Fips";
        var pet = new Pet(name, new DateOnly(2024, 5, 6));
        IDictionary<string, object?>? changes = null;
        var newBirthday = new DateOnly(2025, 6, 7);

        // Act
        var p2 = pet.NewWith(x => x.Birthday, newBirthday).Build(x => changes = x);

        // Assert
        var eq = ReferenceEquals(pet, p2);
        eq.ShouldBeFalse();

        p2.Birthday.ShouldBe(newBirthday);
        p2.Name.ShouldBe(name);

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(1);

        var change = changes.First();
        var expected = new KeyValuePair<string, object?>(nameof(Pet.Birthday), newBirthday);
        change.ShouldBe(expected);
    }
}
