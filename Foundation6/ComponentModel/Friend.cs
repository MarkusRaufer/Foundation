namespace Foundation.ComponentModel;

public static class Friend
{
    public static Friend<TContext> New<TContext>(object friendId, TContext context)
        where TContext : notnull
        => new(friendId.ThrowIfNull(), context.ThrowIfNull());

    public static Friend<TFriendId, TContext> New<TFriendId, TContext>(TFriendId friendId, TContext context)
        where TContext : notnull
        => new(friendId.ThrowIfNull(), context.ThrowIfNull());
};

public record Friend<TContext>(object FriendId, TContext Context) 
    : Friend<object, TContext>(FriendId, Context)
    , IFriend;

public record Friend<TFriendId, TContext>(TFriendId FriendId, TContext Context) : IFriend<TFriendId>;
