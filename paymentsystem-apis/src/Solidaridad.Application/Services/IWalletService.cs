using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Wallet;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IWalletService
{
    Task<CreateWalletResponseModel> CreateAsync(CreateWalletModel model);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<WalletResponseModel>> GetAllAsync(WalletSearchParams searchParams);

    Task<UpdateWalletResponseModel> UpdateAsync(Guid id, UpdateWalletModel model);
}
