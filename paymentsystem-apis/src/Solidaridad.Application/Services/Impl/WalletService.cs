using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Application.Models.Wallet;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Wallets;
using Solidaridad.DataAccess.Repositories;

namespace Solidaridad.Application.Services.Impl;

public class WalletService : IWalletService
{
    private readonly IMapper _mapper;
    private readonly IWalletRepository _walletRepository;
    public WalletService( IMapper mapper, IWalletRepository walletRepository)
    {
        _mapper = mapper;
        _walletRepository = walletRepository;

    }
    public async Task<CreateWalletResponseModel> CreateAsync(CreateWalletModel model)
    {
        try
        {
            var wallet = _mapper.Map<Wallet>(model);
            var addedWallet = await _walletRepository.AddAsync(wallet);

            return new CreateWalletResponseModel
            {
                Id = addedWallet.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var wallet = await _walletRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _walletRepository.DeleteAsync(wallet)).Id
        };
    }

    public async Task<IEnumerable<WalletResponseModel>> GetAllAsync(WalletSearchParams searchParams)
    {
        try
        {
           
            var _wallets = await _walletRepository.GetAllAsync(c => 1==1);
            var wallets = _mapper.Map<IEnumerable<WalletResponseModel>>(_wallets);

            return wallets;


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<UpdateWalletResponseModel> UpdateAsync(Guid id, UpdateWalletModel model)
    {
        try
        {

            var _wallet = await _walletRepository.GetAllAsync(c => c.Id == id);
            var wallet = _wallet.FirstOrDefault();
                _mapper.Map(model, wallet);

            return new UpdateWalletResponseModel
            {
                Id = (await _walletRepository.UpdateAsync(wallet)).Id
            };


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
