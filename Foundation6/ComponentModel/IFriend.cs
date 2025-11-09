namespace Foundation.ComponentModel;

public interface IFriend : IFriend<object>
{
}

public interface IFriend<TFriendId>
{
    TFriendId FriendId { get; }
}