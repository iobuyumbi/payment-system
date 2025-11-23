using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Role;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;

namespace Solidaridad.Application.Services.Impl;

public class RoleService : IRoleService
{
     private readonly IRoleRepository  _roleRepository;
    private readonly IMapper _mapper;
    public RoleService(IRoleRepository roleRepository, IMapper mapper
        ) 
    { 
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<CreateRoleResponseModel> CreateAsync(RoleResponseModel project)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RoleResponseModel>> GetAllAsync(RoleSearchParams searchParams)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RoleResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<UpdateRoleResponseModel> UpdateAsync(Guid id, RoleResponseModel projectModel)
    {
        throw new NotImplementedException();
    }
}
