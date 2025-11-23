using System.Security.Cryptography;

namespace Solidaridad.Application.Helpers;

public static class PasswordGenerator
{
    private const string LOWER_CASE = "abcdefghijklmnopqrstuvwxyz";
    private const string UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NUMBERS = "0123456789";
    private const string SPECIAL_CHARACTERS = "!@#$%^&*()_+-=";

    public static string GeneratePassword(int length)
    {
        if (length < 6)
        {
            throw new ArgumentException("Password length must be at least 6 characters.");
        }

        var passwordChars = new List<char>();

        // Ensure at least one from each required category
        passwordChars.Add(GetRandomChar(LOWER_CASE));
        passwordChars.Add(GetRandomChar(UPPER_CASE));
        passwordChars.Add(GetRandomChar(NUMBERS));
        passwordChars.Add(GetRandomChar(SPECIAL_CHARACTERS));

        int remainingLength = length - passwordChars.Count;

        string allChars = LOWER_CASE + UPPER_CASE + NUMBERS + SPECIAL_CHARACTERS;

        for (int i = 0; i < remainingLength; i++)
        {
            passwordChars.Add(GetRandomChar(allChars));
        }

        // Shuffle the characters
        Shuffle(passwordChars);

        // Ensure at least 1 unique character
        if (passwordChars.Distinct().Count() < 1)
        {
            return GeneratePassword(length); // regenerate if uniqueness fails
        }

        return new string(passwordChars.ToArray());
    }

    private static char GetRandomChar(string from)
    {
        byte[] buffer = new byte[4];
        RandomNumberGenerator.Fill(buffer);
        var rng = new Random(BitConverter.ToInt32(buffer, 0));
        return from[rng.Next(from.Length)];
    }

    private static void Shuffle(List<char> list)
    {
        byte[] buffer = new byte[4];
        RandomNumberGenerator.Fill(buffer);
        var rng = new Random(BitConverter.ToInt32(buffer, 0));

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
