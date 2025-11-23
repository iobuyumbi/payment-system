using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;
using System.Security.Cryptography;
using System.Text;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    #region DI
    protected readonly DbSet<UserCountry> userCountrySet;
    protected readonly DbSet<Country> countrySet;
    protected readonly DbSet<UserOtp> userOtpSet;

    public UserRepository(DatabaseContext context) : base(context)
    {
        userCountrySet = context.Set<UserCountry>();
        countrySet = context.Set<Country>();
        userOtpSet = context.Set<UserOtp>();
    }
    #endregion

    #region Methods
    public async Task<List<UserCountry>> AddUserCountryAsync(string userId, List<Guid> countryIds)
    {
        var userCountries = countryIds.Select(countryId => new UserCountry
        {
            UserId = userId,
            CountryId = countryId
        }).ToList();

        await userCountrySet.AddRangeAsync(userCountries);
        await Context.SaveChangesAsync();

        return userCountries;
    }

    public async Task<List<UserCountry>> UpdateUserCountryAsync(string userId, List<Guid> countryIds)
    {
        // Get distinct countryIds and ensure they exist
        var validCountryIds = await countrySet
            .Where(c => countryIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        // Avoid duplicates
        var existingMappings = await userCountrySet
            .Where(uc => uc.UserId == userId && validCountryIds.Contains(uc.CountryId))
            .Select(uc => uc.CountryId)
            .ToListAsync();

        var newCountryIds = validCountryIds.Except(existingMappings).ToList();

        var newUserCountries = newCountryIds.Select(cid => new UserCountry
        {
            UserId = userId,
            CountryId = cid
        }).ToList();

        if (newUserCountries.Any())
        {
            await userCountrySet.AddRangeAsync(newUserCountries);
            await Context.SaveChangesAsync();
        }

        return newUserCountries;
    }

    public async Task<string> GetUserCountriesStr(string userId)
    {
        var countryNames = await (
            from fp in userCountrySet
            join p in countrySet on fp.CountryId equals p.Id
            where fp.UserId == userId
            select p.CountryName
        ).ToListAsync();

        return string.Join(", ", countryNames);
    }

    public async Task<IEnumerable<Country>> GetUserCountries(string userId)
    {
        var selectItems = from fp in userCountrySet
                          join p in countrySet on fp.CountryId equals p.Id
                          where fp.UserId == userId
                          select new Country
                          {
                              CountryName = p.CountryName,
                              Code = p.Code,
                              CurrencyName = p.CurrencyName,
                              CurrencyPrefix = p.CurrencyPrefix,
                              Id = p.Id
                          };

        return selectItems;
    }

    public string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // 6-digit
    }

    private string HashOtp(string otp)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(otp);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public async Task<string> CreateAndStoreOtpAsync(string userId)
    {
        string otp = GenerateOtp();
        string otpHash = HashOtp(otp);

        var userOtp = new UserOtp
        {
            UserId = userId,
            OtpHash = otpHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false,
            AttemptCount = 0
        };

        await userOtpSet.AddAsync(userOtp);
        await Context.SaveChangesAsync();

        return otp;
    }

    public async Task<bool> VerifyOtpAsync(string userId, string enteredOtp)
    {
        string otpHash = HashOtp(enteredOtp);

        var otpRecord = await userOtpSet
            .Where(o => o.UserId == userId && !o.IsUsed && o.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (otpRecord == null) return false;

        // Retry limit
        otpRecord.AttemptCount++;
        if (otpRecord.AttemptCount > 3)
        {
            await Context.SaveChangesAsync();
            return false;
        }

        if (otpRecord.OtpHash == otpHash)
        {
            otpRecord.IsUsed = true;
            await Context.SaveChangesAsync();
            return true;
        }

        await Context.SaveChangesAsync();
        return false;
    }


    #endregion
}
