using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.ComponentModel;

[TestFixture]

public class DelegateOwnerTests
{
    private record SetState(Action<string> Delegate) : IHasDelegate<Action<string>>;

    private class DelegateOwner : IDelegateOwner<SetState, Action<string>>
    {
        private string _state;
        private SetState? _setState;

        public DelegateOwner(string state)
        {
            _state = state;
            _setState = new SetState(SetInternalState);
        }

        private void SetInternalState(string state)
        {
            _state = state;
        }

        public string State => _state;
        public bool HasDelegateOwnerShip => _setState != null;

        public SetState? MoveDelegateOwnerShip()
        {
            var d = _setState;
            _setState = null;
            return d;
        }
    }

    [Test]
    public void DynamicInvoke_Should_CallDelegate_When_UsingTyped_IDelegateOwner()
    {
        //Arrange
        var owner = new DelegateOwner("123");
        var owned = owner.MoveDelegateOwnerShip();

        // Act
        owned.ShouldNotBeNull();
        owned.Delegate("456");
        var owned2 = owner.MoveDelegateOwnerShip();

        // Assert
        owner.State.ShouldBe("456");
        owned2.ShouldBeNull();
    }
}
