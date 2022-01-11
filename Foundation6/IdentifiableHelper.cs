namespace Foundation;

public static class IdentifiableHelper
{
    public static bool Equals<T>(IIdentifiable<T>? lhs, IIdentifiable<T>? rhs)
        where T : notnull
    {
        if (lhs is null) return rhs is null;
        if (rhs is null) return false;

        return lhs.Id.Equals(rhs.Id);
    }
}

