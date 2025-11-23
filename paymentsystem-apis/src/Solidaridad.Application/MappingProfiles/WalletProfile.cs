using AutoMapper;
using Solidaridad.Application.Models.Wallet;
using Solidaridad.Core.Entities.Wallets;

namespace Solidaridad.Application.MappingProfiles;

public class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<Wallet, WalletResponseModel>();
       
        CreateMap<CreateWalletModel, Wallet>();
        
        CreateMap<WalletResponseModel, Wallet>();
        
        CreateMap<UpdateWalletModel, Wallet>();
    }
}


