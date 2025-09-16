namespace Foundation.ComponentModel;

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
