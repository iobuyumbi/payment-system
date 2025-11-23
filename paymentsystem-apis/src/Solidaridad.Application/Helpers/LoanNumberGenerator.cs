namespace Solidaridad.Application.Helpers;

public class LoanNumberGenerator
{
    private static readonly Random random = new Random();

    public static string GenerateLoanNumber(string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            throw new ArgumentException("Country code must be exactly 2 characters.");

        string year = DateTime.UtcNow.ToString("yy"); // Last 2 digits of the year
        string monthDay = DateTime.UtcNow.ToString("MMdd"); // MMDD format
        string uniqueId = random.Next(100000, 999999).ToString(); // 6-digit unique identifier

        return $"{countryCode.ToUpper()}{year}{monthDay}{uniqueId}";
    }
}
