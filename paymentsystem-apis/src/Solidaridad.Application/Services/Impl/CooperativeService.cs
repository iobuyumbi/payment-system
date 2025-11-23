using AutoMapper;
using FluentValidation;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Cooperative;
using Solidaridad.Application.Models.Validators.AdminLevel3;
using Solidaridad.Application.Models.Validators.Cooperative;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;

using Solidaridad.DataAccess.Repositories;

namespace Solidaridad.Application.Services.Impl;

public class CooperativeService : ICooperativeService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly ICooperativeRepository _cooperativeRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly CreateCooperativeValidator _createCooperativeValidator;
    private readonly UpdateCooperativeValidator _updateCooperativeValidator;
    public CooperativeService(IMapper mapper, ICooperativeRepository cooperativeRepository, ICountryRepository countryRepository)
    {
        _cooperativeRepository = cooperativeRepository;
        _mapper = mapper;
        _countryRepository = countryRepository;
        _createCooperativeValidator= new CreateCooperativeValidator();
        _updateCooperativeValidator = new UpdateCooperativeValidator();
    }
    #endregion

    #region Methods
    public async Task<CreateCooperativeResponseModel> CreateAsync(CreateCooperativeModel createCooperativeModel)
    {
        try
        {
            var validationResult = _createCooperativeValidator.Validate(createCooperativeModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }
            var existingCooperative = await _cooperativeRepository.GetAllAsync(c => c.Name.ToLower()
            .Equals(createCooperativeModel.Name.ToLower()));

            if (existingCooperative.Any())
            {

                throw new InvalidOperationException("A cooperative with the same name already exists.");
            }
            var cooperative = _mapper.Map<Cooperative>(createCooperativeModel);
            cooperative.Id = Guid.NewGuid();
            var addedCooperative = await _cooperativeRepository.AddAsync(cooperative);

            return new CreateCooperativeResponseModel
            {
                Id = addedCooperative.Id
            };
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<CreateCooperativeResponseModel> ImportAsync(ImportCoperativeModel importCoperativeModel)
    {
        try
        {
            // check duplicates
            var existingCooperative = await _cooperativeRepository.GetAllAsync(c => c.Name.ToLower().Equals(importCoperativeModel.Name.ToLower()) );

            if (existingCooperative.Any())
            {
                throw new InvalidOperationException("A cooperative with the same name already exists.");
            }

            // add cooperative
            var cooperative = _mapper.Map<Cooperative>(importCoperativeModel);
            var country = await _countryRepository.GetAllAsync(c => c.CountryName.ToLower().Equals(importCoperativeModel.CountryName.ToLower()));

            if (country.Any())
            {
                cooperative.CountryId= country.FirstOrDefault().Id;
            }
            cooperative.Id = Guid.NewGuid();
            var addedCooperative = await _cooperativeRepository.AddAsync(cooperative);

            return new CreateCooperativeResponseModel
            {
                Id = addedCooperative.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var cooperative = await _cooperativeRepository.GetFirstAsync(tl => tl.Id == id );
        cooperative.IsDeleted = true;
        return new BaseResponseModel
        {
            Id = (await _cooperativeRepository.UpdateAsync(cooperative)).Id
        };
    }

    public async Task<IEnumerable<CooperativeResponseModel>> GetAllAsync(CooperativeSearchParams cooperativeSearchParams)
    {
        var _cooperative = await _cooperativeRepository.GetAllAsync(c =>
              (  string.IsNullOrEmpty(cooperativeSearchParams.Filter) ||
                c.Name.Contains(cooperativeSearchParams.Filter)
            )&& c.CountryId == cooperativeSearchParams.CountryId && c.IsDeleted == false);
        var _countries = await _countryRepository.GetAllAsync(c => c.IsActive == true && c.IsDeleted == false);
        //if (cooperativeSearchParams.CountryId != null)
        //{
        //    var filteredProjects = _cooperative.Where(p => p.CountryId == cooperativeSearchParams.CountryId).ToList();
        //    _cooperative = filteredProjects;

        //}


        int numberOfObjectsPerPage = cooperativeSearchParams.PageSize;

        var queryResultPage = _cooperative
            .Skip(numberOfObjectsPerPage * (cooperativeSearchParams.PageNumber - 1))
            .Take(numberOfObjectsPerPage);

        queryResultPage.ToList().ForEach(f => f.Country = _countries.FirstOrDefault(c => c.Id == f.CountryId));

        return _mapper.Map<IEnumerable<CooperativeResponseModel>>(queryResultPage.OrderBy(c => c.Name));
    }

    public async Task<UpdateCooperativeResponseModel> UpdateAsync(Guid id, UpdateCooperativeModel updateCooperativeModel)
    {
        try
        {
            var validationResult = _updateCooperativeValidator.Validate(updateCooperativeModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }
            var cooperative = await _cooperativeRepository.GetFirstAsync(ti => ti.Id == id && ti.IsDeleted == false);

        _mapper.Map(updateCooperativeModel, cooperative);

        return new UpdateCooperativeResponseModel
        {
            Id = (await _cooperativeRepository.UpdateAsync(cooperative)).Id
        };
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
}
