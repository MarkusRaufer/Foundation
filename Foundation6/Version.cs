#if NET8_0_OR_GREATER

using Foundation.Buffers;

#endif

namespace Foundation;

/// <summary>
/// This version object corresponds to semantic versioning.
/// </summary>
/// <param name="Major"></param>
/// <param name="Minor"></param>
/// <param name="Patch"></param>
public record struct Version(int Major, int Minor, int Patch)
{
    /// <summary>
    /// Increments the major part of the version.
    /// </summary>
    /// <returns></returns>
    public readonly Version IncrementMajor() => new(Major + 1, Minor, Patch);

    /// <summary>
    /// Increments the minor part of the version.
    /// </summary>
    /// <returns></returns>
    public readonly Version IncrementMinor() => new(Major, Minor + 1, Patch);

    /// <summary>
    /// Increments the patch part of the version.
    /// </summary>
    /// <returns></returns>
    public readonly Version IncrementPatch() => new(Major, Minor, Patch + 1);

    public static Version? Parse(string version)
    {
        if (string.IsNullOrWhiteSpace(version)) return null;

        var span = version.AsSpan();

        ReadOnlySpan<char> separators = stackalloc char[1] { '.' };

#if NET8_0_OR_GREATER
        var enumerator = span.Split(separators);

        //major
        if (!enumerator.MoveNext()) return null;

        if (!int.TryParse(enumerator.Current, out var major)) return null;

        //minor
        if (!enumerator.MoveNext()) return null;

        if (!int.TryParse(enumerator.Current, out var minor)) return null;

        //patch
        if (!enumerator.MoveNext()) return null;

        if (!int.TryParse(enumerator.Current, out var patch)) return null;

        return new Version(major, minor, patch);
#else
        var parts = version.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3) return null;
        
        //major
        if (!int.TryParse(parts[0], out var major)) return null;

        //minor
        if (!int.TryParse(parts[1], out var minor)) return null;

        //patch
        if (!int.TryParse(parts[2], out var patch)) return null;

        return new Version(major, minor, patch);
#endif

    }

    public static bool TryParse(string str, out Version? version)
    {
        version = Parse(str);
        return version is not null;
    }


    public override string ToString() => $"{Major}.{Minor}.{Patch}";
}
