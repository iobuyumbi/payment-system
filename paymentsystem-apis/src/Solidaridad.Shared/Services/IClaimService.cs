namespace Solidaridad.Shared.Services
{
    public interface IClaimService
    {
        string GetUserId();

        string GetClaim(string key);

        string GetUserEmail();
    }
}
