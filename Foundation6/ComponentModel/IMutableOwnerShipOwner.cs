namespace Foundation.ComponentModel;

/// <summary>
/// Contract of an object that has artifacts which ownership can be moved.
/// </summary>
public interface IMutableOwnerShipOwner
{
    /// <summary>
    /// True if the owner has still the ownership otherwise false.
    /// </summary>
    /// <typeparam name="T">The type of the owned object.</typeparam>
    /// <returns></returns>
    bool HasOwnerShip<T>();


    /// <summary>
    /// Moves the ownership. Calling this method a second time returns null.
    /// <typeparam name="T">The type of the owned object.</typeparam>
    /// </summary>
    T? MoveOwnerShip<T>();
}

/// <summary>
/// Contract of an object that has an artifact which ownership can be moved.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMutableOwnerShipOwner<T>
{
    /// <summary>
    /// True if the owner has still the ownership otherwise false.
    /// </summary>
    /// <typeparam name="T">The type of the owned object.</typeparam>
    /// <returns></returns>
    bool HasOwnerShip { get; }


    /// <summary>
    /// Moves the ownership. Calling this method a second time returns null.
    /// <typeparam name="T">The type of the owned object.</typeparam>
    /// </summary>
    T? MoveOwnerShip();
}
