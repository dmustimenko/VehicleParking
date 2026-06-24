namespace VehicleParking.Domain.Helpers;

public static class VehicleRegHelpers
{
    private const int StackAllocThreshold = 64;

    public static string Normalize(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        ReadOnlySpan<char> source = value.AsSpan();
        Span<char> buffer = source.Length <= StackAllocThreshold
            ? stackalloc char[source.Length]
            : new char[source.Length];

        var length = 0;
        foreach (char character in source)
        {
            if (!char.IsWhiteSpace(character))
            {
                buffer[length++] = char.ToUpperInvariant(character);
            }
        }

        return buffer[..length].ToString();
    }
}
