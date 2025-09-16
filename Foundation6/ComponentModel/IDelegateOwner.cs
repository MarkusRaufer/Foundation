namespace Foundation.ComponentModel;

/// <summary>
/// Contract of a replace delegate owner. After calling <see cref="MoveReplaceDelegate"/> the owner loses the ownership.
/// Calling <see cref="MoveReplaceDelegate"/> a second time returns null.
/// </summary>
/// <typeparam name="T">The type of the delegate's input value.</typeparam>
public interface IDelegateOwner<T, TDelegate>
    where T : IHasDelegate<TDelegate>
    where TDelegate : Delegate
{
    /// <summary>
    /// True if the object has still the ownership otherwise false.
    /// </summary>
    bool HasDelegateOwnerShip { get; }

    /// <summary>
    /// Moves the replace delegate's owner ship. Calling this method a second time returns null.
    /// </summary>
    T? MoveDelegateOwnerShip();
}
