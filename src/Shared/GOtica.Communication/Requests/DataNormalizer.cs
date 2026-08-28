namespace GOtica.Communication.Requests;

internal static class DataNormalizer
{
    public static string Text(string value)
    {
        return value.Trim();
    }

    public static string Email(string value)
    {
        return value.Trim().ToLowerInvariant();
    }

    public static string? OptionalEmail(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return Email(value);
    }

    public static string PhoneNumber(string value)
    {
        return value.Trim();
    }

    public static string TaxNumber(string value)
    {
        return value.Trim();
    }

    public static string Role(string value)
    {
        return value.Trim().ToLowerInvariant();
    }

    public static string ProductCode(string value)
    {
        return value.Trim().ToUpperInvariant();
    }

    public static string? OptionalPhoneNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return PhoneNumber(value);
    }
}