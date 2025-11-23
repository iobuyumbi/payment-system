using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Role;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Enums;
using Solidaridad.DataAccess.Identity;

namespace Solidaridad.API.Controllers;

[Authorize]
public class RolesController : ApiController
{
    #region DI
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ICountryService _countryService;

    public RolesController(RoleManager<ApplicationRole> roleManager, ICountryService countryService)
    {
        _roleManager = roleManager;
        _countryService = countryService;
    }
    #endregion

    #region Methods
    [HttpGet]

    public async Task<IActionResult> GetAll()
    {
        // hide superadmin and prevent modifications
        return Ok(ApiResult<IEnumerable<ApplicationRole>>.Success(
                    await _roleManager.Roles.Where(c =>
                    // c.CountryId == CountryId &&
                    !c.Name.Equals(Roles.SuperAdmin.ToString())).ToListAsync())
                );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(string id)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(c => c.Id.Equals(id));

        return Ok(ApiResult<ApplicationRole>.Success(role));
    }
    [HttpPost]
    public async Task<IActionResult> AddRole([FromBody] CreateRoleModel roleModel)
    {
        var results = new List<IdentityResult>();
        var countries = await _countryService.GetAllAsync();
        foreach (var countryId in roleModel.CountryIds)
        {
            var code = countries.FirstOrDefault(c => c.Id == countryId).Code;
            var role = new ApplicationRole(roleModel.Name.Trim() + "-" + code)
            {
                // Example: Add a CountryId field in ApplicationRole if it exists.
                CountryId = countryId
            };

            var result = await _roleManager.CreateAsync(role);
            results.Add(result);
        }

        return Ok(ApiResult<List<IdentityResult>>.Success(results));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Role name cannot be empty.");
        }

        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound("Role not found.");
        }

        role.Name = name.Trim();
        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded)
        {
            return BadRequest(ApiResult<IdentityResult>.Failure(result.Errors.Select(e => e.Description).ToArray()));
        }

        return Ok(ApiResult<IdentityResult>.Success(result));
    }

    #endregion
}

