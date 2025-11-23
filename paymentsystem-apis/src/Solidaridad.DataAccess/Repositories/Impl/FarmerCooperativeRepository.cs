using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class FarmerCooperativeRepository: BaseRepository<FarmerCooperative>, IFarmerCooperativeRepository
{
     public FarmerCooperativeRepository(DatabaseContext context) : base(context){}
}
