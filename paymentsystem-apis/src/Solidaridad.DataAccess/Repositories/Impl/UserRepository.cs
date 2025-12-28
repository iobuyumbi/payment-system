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
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(enteredOtp)) return false;

        var normalizedUserId = userId.Trim();
        var normalizedOtp = enteredOtp.Trim();
        var otpHash = HashOtp(normalizedOtp);

        var activeOtps = await userOtpSet
            .Where(o => o.UserId == normalizedUserId && !o.IsUsed && o.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(o => o.CreatedAt)
            .Take(10)
            .ToListAsync();

        if (activeOtps.Count == 0) return false;

        // Accept any active OTP for the user (helps when multiple OTPs were generated and the user enters an earlier one)
        var matchingRecord = activeOtps.FirstOrDefault(o => o.OtpHash == otpHash);
        if (matchingRecord != null)
        {
            matchingRecord.IsUsed = true;
            await Context.SaveChangesAsync();
            return true;
        }

        // Retry limit is tracked on the latest OTP record
        var latestRecord = activeOtps[0];
        latestRecord.AttemptCount++;
        if (latestRecord.AttemptCount > 3)
        {
            await Context.SaveChangesAsync();
            return false;
        }

        await Context.SaveChangesAsync();
        return false;
    }


    #endregion
}
