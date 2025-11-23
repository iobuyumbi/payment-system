using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Wallets;
using Solidaridad.DataAccess.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
{
    public WalletRepository(DatabaseContext context) : base(context)
    {
    }
}
