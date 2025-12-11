using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Solidaridad.Application.Helpers;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Permission;
using Solidaridad.Application.Models.RolePermission;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Excel.Import;
using Solidaridad.DataAccess.Identity;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;

namespace Solidaridad.Application.Services.Impl;

public class PermissionService : BaseService, IPermissionService
{
    #region DI
    private readonly IClaimService _claimService;
    private readonly IMapper _mapper;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IModuleRepository _moduleRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public PermissionService(IPermissionRepository permissionRepository,
        IModuleRepository moduleRepository,
        IMapper mapper,
        IClaimService claimService,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IConfiguration configuration) : base(configuration)
    {
        _permissionRepository = permissionRepository;
        _moduleRepository = moduleRepository;
        _mapper = mapper;
        _claimService = claimService;
        _roleManager = roleManager;
        _userManager = userManager;
    }
    #endregion

    #region Permission Methods

    public async Task<CreatePermissionResponseModel> CreateAsync(CreatePermissionModel createPermissionModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = _mapper.Map<Permission>(createPermissionModel);

            var addedPermission = await _permissionRepository.AddAsync(permission);

            return new CreatePermissionResponseModel
            {
                Id = addedPermission.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _permissionRepository.DeleteAsync(permission)).Id
        };
    }

    public async Task<IEnumerable<PermissionResponseModel>> GetAllAsync(string roleId, Guid orgId, CancellationToken cancellationToken = default)
    {
        using (var connection = GetConnection())
        {
            await connection.OpenAsync();
            // Direct SQL query instead of function call (function doesn't exist in PostgreSQL)
            var roleIdGuid = Guid.Parse(roleId);
            var sql = @"
                SELECT 
                    p.""Id"",
                    COALESCE(p.""Description"", '') AS ""PermissionDescription"",
                    p.""PermissionName"",
                    '' AS ""ModuleName"",
                    CASE WHEN rp.""Id"" IS NOT NULL THEN true ELSE false END AS ""Selected""
                FROM 
                    ""Permission"" p
                LEFT JOIN 
                    ""RolePermission"" rp ON p.""Id"" = rp.""PermissionId"" 
                    AND rp.""RoleId""::uuid = @RoleIdGuid
                    AND rp.""IsDeleted"" = false
                WHERE 
                    p.""IsDeleted"" = false
                ORDER BY 
                    p.""PermissionName""";
            
            return await connection.QueryAsync<PermissionResponseModel>(sql,
                new
                {
                    RoleIdGuid = roleIdGuid
                });
        }
    }

    public async Task<UpdatePermissionResponseModel> UpdateAsync(Guid id, UpdatePermissionModel updatePermissionModel, CancellationToken cancellationToken = default)
    {
        var permission = await _permissionRepository.GetFirstAsync(tl => tl.Id == id);

        var orgId = Guid.Parse(_claimService.GetClaim("orgid"));

        permission.PermissionName = updatePermissionModel.PermissionName;

        return new UpdatePermissionResponseModel
        {
            Id = (await _permissionRepository.UpdateAsync(permission)).Id
        };
    }

    #endregion

    #region Role Permissions

    public async Task<bool> CheckUserPermissionAsync(string userId, string permissionName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var roles = await _userManager.GetRolesAsync(user);

        //var hasPermission = await roles.ToList()
        //    .AnyAsync(ur => ur.UserId == userId &&
        //                    ur.Role.RolePermissions.Any(rp => rp.Permission.PermissionName == permissionName));
        //return hasPermission;

        return false;
    }

    public async Task<UpdateRolePermissionResponseModel> UpdateRolePermissionAsync(Guid roleId, IEnumerable<UpdateRolePermissionModel> updateRolePermissionModel, CancellationToken cancellationToken = default)
    {
        // Convert the list to JSON
        string json = JsonConvert.SerializeObject(updateRolePermissionModel);

        using (var connection = GetConnection())
        {
            await connection.OpenAsync();

            return await connection.QuerySingleAsync<UpdateRolePermissionResponseModel>("CALL MergeRolePermissions(@roleId,@json,'')",
                new
                {
                    roleId = roleId,
                    json = json,

                });

        }
    }

    #endregion

    #region Import
    public async Task ImportPermission(IFormFile file, Guid? id)
    {
        try
        {
            XSSFWorkbook hssfwb;
            using (var stream = file.OpenReadStream())
            {
                hssfwb = new XSSFWorkbook(stream);
            }

            ISheet sheet = hssfwb.GetSheet(Convert.ToString(ExelImportConstants.SHEET_PERMISSIONS));
            var importedPermissions = new List<ImportPermissionModel>();
            var _listPermissions = new List<Permission>();
            var _errorDetails = new List<ExcelImportDetail>();

            // check if sheet exists
            if (sheet != null)
            {
                DataFormatter formatter = new DataFormatter();
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                    {
                        importedPermissions.Add(new ImportPermissionModel
                        {
                            ModuleName = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(0))),
                            PermissionName = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(2))),
                        });
                    }
                }
                try
                {
                    string lastModuleName = null;
                    foreach (var permission in importedPermissions)
                    {
                        if (!string.IsNullOrWhiteSpace(permission.ModuleName))
                        {
                            lastModuleName = permission.ModuleName.Trim();
                        }
                        else
                        {
                            permission.ModuleName = lastModuleName; // fill down
                        }
                    }


                    var permissions = await _permissionRepository.GetAllAsync(c => 1 == 1);

                    // Fetch existing modules
                    var modules = await _moduleRepository.GetAllAsync(c => true);

                    var distinctModuleNames = importedPermissions
                                .Select(p => p.ModuleName?.Trim()) // use ?.Trim() to avoid null exception
                                .Where(name => !string.IsNullOrWhiteSpace(name))
                                .Distinct()
                                .ToList();


                    var existingModules = modules
                        .Where(m => distinctModuleNames.Contains(m.ModuleName))
                        .ToList();

                    // Determine new modules to insert
                    var newModules = distinctModuleNames
                        .Except(existingModules.Select(m => m.ModuleName))
                        .Select(name => new Module { ModuleName = name })
                        .ToList();


                    if (newModules.Any())
                    {
                        await _moduleRepository.AddRange(newModules);
                    }

                    // Combine all modules (existing + newly inserted)
                    var allModules = existingModules.Concat(newModules).ToList();

                    // Prepare permissions to insert
                    var existingPermissionNames = permissions
                        .Select(p => p.PermissionName)
                        .ToList();

                    var newPermissions = importedPermissions
                            .Where(p => !existingPermissionNames.Contains(p.PermissionName))
                            .Select(p =>
                            {
                                var module = allModules.FirstOrDefault(m => m.ModuleName == p.ModuleName?.Trim());
                                if (module == null)
                                {
                                    throw new Exception($"Module '{p.ModuleName}' not found for Permission '{p.PermissionName}'");
                                }
                                return new Permission
                                {
                                    PermissionName = p.PermissionName,
                                    ModuleId = module.Id,
                                    Description = GenerateDescription(p.PermissionName) // <-- new
                                };
                            })
                            .ToList();


                    if (newPermissions.Any())
                    {
                        await _permissionRepository.AddRange(newPermissions);
                    }

                }
                catch (Exception ex) { }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    private string GenerateDescription(string permissionName)
    {
        if (string.IsNullOrWhiteSpace(permissionName))
            return string.Empty;

        var parts = permissionName.Split('.');

        var readableParts = parts
            .Select(part => SplitCamelCase(part)) // e.g., viewcounters => View Counters
            .ToList();

        return string.Join(" - ", readableParts);
    }

    private string SplitCamelCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var result = System.Text.RegularExpressions.Regex.Replace(
            input,
            "(\\B[A-Z])",
            " $1"
        );

        // Capitalize first letter
        return char.ToUpper(result[0]) + result.Substring(1);
    }


    //#region DI
    //private readonly IClaimService _claimService;
    //private readonly IMapper _mapper;
    //private readonly IPermissionRepository _permissionRepository;
    //private readonly UserManager<ApplicationUser> _userManager;
    //private readonly RoleManager<ApplicationRole> _roleManager;

    //public PermissionService(IPermissionRepository permissionRepository, IMapper mapper, IClaimService claimService,
    //    UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    //{
    //    _permissionRepository = permissionRepository;
    //    _mapper = mapper;
    //    _claimService = claimService;
    //    _roleManager = roleManager;
    //    _userManager = userManager;
    //}
    //#endregion

    //#region Methods
    //public async Task<CreatePermissionResponseModel> CreateAsync(CreatePermissionModel createPermissionModel, CancellationToken cancellationToken = default)
    //{
    //    try
    //    {
    //        var permission = _mapper.Map<Solidaridad.Core.Entities.Permission>(createPermissionModel);

    //        var addedPermission = await _permissionRepository.AddAsync(permission);

    //        return new CreatePermissionResponseModel
    //        {
    //            Id = addedPermission.Id
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public async Task<BaseResponseModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    //{
    //    var permission = await _permissionRepository.GetFirstAsync(tl => tl.Id == id);

    //    return new BaseResponseModel
    //    {
    //        Id = (await _permissionRepository.DeleteAsync(permission)).Id
    //    };
    //}

    //public async Task<IEnumerable<PermissionResponseModel>> GetAllAsync(string keyword, CancellationToken cancellationToken = default)
    //{
    //    var currentOrgId = Guid.Parse(_claimService.GetClaim("orgid"));

    //    var permissions = await _permissionRepository.GetAllAsync(c => 1 == 1);

    //    if (!string.IsNullOrEmpty(keyword))
    //    {
    //        permissions = permissions.Where(e =>
    //                     CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.PermissionName, keyword, CompareOptions.IgnoreCase) >= 0
    //                ).ToList();
    //    }
    //    return _mapper.Map<IEnumerable<PermissionResponseModel>>(permissions);
    //}

    //public async Task<UpdatePermissionResponseModel> UpdateAsync(Guid id, UpdatePermissionModel updatePermissionModel, CancellationToken cancellationToken = default)
    //{
    //    var permission = await _permissionRepository.GetFirstAsync(tl => tl.Id == id);

    //    var orgId = Guid.Parse(_claimService.GetClaim("orgid"));

    //    permission.PermissionName = updatePermissionModel.PermissionName;

    //    return new UpdatePermissionResponseModel
    //    {
    //        Id = (await _permissionRepository.UpdateAsync(permission)).Id
    //    };
    //}
    //#endregion

    //public async Task<bool> CheckUserPermissionAsync(string userId, string permissionName)
    //{
    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null)
    //    {
    //        return false;
    //    }

    //    var roles = await _userManager.GetRolesAsync(user);

    //    //var hasPermission = await roles.ToList()
    //    //    .AnyAsync(ur => ur.UserId == userId &&
    //    //                    ur.Role.RolePermissions.Any(rp => rp.Permission.PermissionName == permissionName));
    //    //return hasPermission;

    //    return false;
    //}
}
