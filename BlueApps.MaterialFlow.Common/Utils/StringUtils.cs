namespace BlueApps.MaterialFlow.Common.Utils;

public static class StringUtils
{
    private static readonly char[] _lowercaseLettersAndDigits = "abcdefghijklmnopqrstvwxyz0123456789".ToCharArray();

    /// <summary>
    /// Random strings. Useable for ids.
    /// </summary>
    /// <param name="chars">Characters to include in the randomized string</param>
    /// <param name="length">The length of the returned string.</param>
    /// <returns>Random string.</returns>
    /// <exception cref="ArgumentException"/>
    public static string GetRandomString(this char[] chars, int length)
    {
        if (length < 1)
            throw new ArgumentOutOfRangeException(nameof(length), "The length of the string has to be greater than zero");

        return new string(Enumerable.Range(0, length).Select(_ => chars[Random.Shared.Next(chars.Length)]).ToArray());
    }

    /// <summary>
    /// Random strings. Useable for ids.
    /// </summary>
    /// <param name="length">The length of the returned string.</param>
    /// <returns>Random string.</returns>
    /// <exception cref="ArgumentException"/>
    public static string GetRandomString(int length) => GetRandomString(_lowercaseLettersAndDigits, length);
}