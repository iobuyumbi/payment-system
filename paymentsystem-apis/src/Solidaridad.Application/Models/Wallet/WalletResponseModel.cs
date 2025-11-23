namespace Solidaridad.Application.Models.Wallet;

public class WalletResponseModel : BaseResponseModel
{
    public decimal Balance { get; private set; }

    public Guid OwnerId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    
}
