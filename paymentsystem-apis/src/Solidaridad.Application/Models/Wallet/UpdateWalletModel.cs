namespace Solidaridad.Application.Models.Wallet;

public class UpdateWalletModel
{
    public decimal Balance { get; private set; }

    public Guid OwnerId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
public class UpdateWalletResponseModel  : BaseResponseModel { }
