using AutoMapper;
using NPOI.SS.Formula.Functions;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Associate;
using Solidaridad.Application.Models.Cooperative;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Services.Impl;

public class AssociateService : IAssociateService
{
    private readonly IMapper _mapper;
    private IAssociateRepository _associateRepository;
    private IFarmerRepository _farmerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IAdminLevel1Repository _countyRepository;
    private readonly IAdminLevel2Repository _subCountyRepository;
    private readonly IAdminLevel3Repository _wardRepository;
    private readonly IAdminLevel4Repository _villageRepository;
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    public AssociateService (IMapper mapper, IAssociateRepository associateRepository, ICountryRepository countryRepository, IAdminLevel1Repository countyRepository,
        IAdminLevel2Repository subCountyRepository, IAdminLevel3Repository wardRepository,
        IAdminLevel4Repository villageRepository,
        ILoanApplicationRepository loanApplicationRepository, IFarmerRepository farmerRepository)
    { 
        _associateRepository = associateRepository;
        _mapper = mapper;
        _farmerRepository = farmerRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _countyRepository = countyRepository;
        _wardRepository = wardRepository;
        _subCountyRepository = subCountyRepository;
        _villageRepository = villageRepository;
        _countryRepository = countryRepository;
    }
    public async Task<CreateAssociateResponseModel> CreateAsync(CreateAssociateModel createAssociateModel)
    {
        try
        {
            var existingAssociate= await _associateRepository.GetAllAsync(c => c.FarmerId
            .Equals(createAssociateModel.FarmerId));

            if (existingAssociate.Any())
            {

                throw new InvalidOperationException("A associate with the same name already exists.");
            }
            var associate = _mapper.Map<AssociateMap>(createAssociateModel);
            associate.Id = Guid.NewGuid();
            var addedAssociate = await _associateRepository.AddAsync(associate);

            return new CreateAssociateResponseModel
            {
                Id = addedAssociate.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<IEnumerable<CreateAssociateResponseModel>> MultiAdd(IEnumerable<CreateAssociateModel> createAssociateModel)
    {
        try
        {
            var createAssociateResponseModel = new List<CreateAssociateResponseModel>();
            foreach (var _associate in createAssociateModel)
            {
                var existingAssociate = await _associateRepository.GetAllAsync(c => c.FarmerId
            .Equals(_associate.FarmerId));

                if (existingAssociate.Any())
                {

                    throw new InvalidOperationException("A associate with the same name already exists.");
                }
                var associate = _mapper.Map<AssociateMap>(_associate);
                associate.Id = Guid.NewGuid();
                var addedAssociate = await _associateRepository.AddAsync(associate);

                var responseModel = new CreateAssociateResponseModel
                {
                    Id = addedAssociate.Id,
                    
                };

                createAssociateResponseModel.Add(responseModel);
            }
            return createAssociateResponseModel;
            

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var cooperative = await _associateRepository.GetFirstAsync(tl => tl.Id == id);
        cooperative.IsDeleted = true;

        return new BaseResponseModel
        {
            Id = (await _associateRepository.DeleteAsync(cooperative)).Id
        };
    }

    public async Task<ReadOnlyCollection<AssociateResponseModel>> GetAllAsync()
    {
        var _associate = await _associateRepository.GetAllAsync(c => c.IsDeleted == false);
      

        return (ReadOnlyCollection<AssociateResponseModel>)_mapper.Map<IEnumerable<AssociateResponseModel>>(_associate);
    }

    public async Task<ReadOnlyCollection<FarmerResponseModel>> GetAssociatedFarmers(Guid batchId)
    {
        // Get the loan applications
        var _loanApplications = await _loanApplicationRepository.GetAllAsync(c => c.LoanBatchId == batchId && c.IsDeleted == false);

        // Create a dictionary to store unique farmers by their FarmerId
        var farmersDictionary = new Dictionary<Guid, FarmerResponseModel>();

        foreach (var _map in _loanApplications)
        {
            // Check if the farmer is already in the dictionary
            if (!farmersDictionary.ContainsKey(_map.FarmerId))
            {
                // Get the farmer details
                var _farmer = await _farmerRepository.GetFirstAsync(c => c.Id == _map.FarmerId && c.IsDeleted == false);

                if (_farmer != null)
                {
                    // Fetch related administrative data
                    var _countries = await _countryRepository.GetAllAsync(c => c.IsActive == true && c.IsDeleted == false);
                    var _level1 = await _countyRepository.GetAllAsync(c => c.IsActive == true && c.IsDeleted == false);
                    var _level2 = await _subCountyRepository.GetAllAsync(c => c.IsActive == true && c.IsDeleted == false);
                    var _level3 = await _wardRepository.GetAllAsync(c => c.IsActive == true && c.IsDeleted == false);

                    // Map administrative levels
                    _farmer.Country = _countries.FirstOrDefault(c => c.Id == _farmer.CountryId);
                    _farmer.AdminLevel1 = _level1.FirstOrDefault(c => c.Id == _farmer.AdminLevel1Id);
                    _farmer.AdminLevel2 = _level2.FirstOrDefault(c => c.Id == _farmer.AdminLevel2Id);
                    _farmer.AdminLevel3 = _level3.FirstOrDefault(c => c.Id == _farmer.AdminLevel3Id);

                    // Map the farmer to the response model and add to the dictionary
                    var farmerResponse = _mapper.Map<FarmerResponseModel>(_farmer);
                    farmersDictionary.Add(_farmer.Id, farmerResponse);
                }
            }
        }

        // Return only the unique farmers
        return farmersDictionary.Values.ToList().AsReadOnly();
    }


    public async Task<UpdateAssociateResponseModel> UpdateAsync(Guid id, UpdateAssociateModel updateCountryModel)
    {
        var associate = await _associateRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateCountryModel, associate);

        return new UpdateAssociateResponseModel
        {
            Id = (await _associateRepository.UpdateAsync(associate)).Id
        };
    }
}
