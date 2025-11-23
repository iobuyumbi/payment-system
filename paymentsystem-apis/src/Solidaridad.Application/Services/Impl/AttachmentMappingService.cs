using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanAttachmentModel;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;

namespace Solidaridad.Application.Services.Impl;

public class AttachmentMappingService : IAttachmentMappingService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly IAttachmentMappingRepository _attachmentRepository;
   

    public AttachmentMappingService(IMapper mapper, IAttachmentMappingRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
        _mapper = mapper;
       
    }
    #endregion
    public async Task<CreateAttachmentResponseModel> CreateAsync(CreateAttachmentModel createAttachmentModel)
    {
        try
        {
          
            var attachment = _mapper.Map<AttachmentMapping>(createAttachmentModel);
            attachment.Id = Guid.NewGuid();
            var addedAttachmentMap = await _attachmentRepository.AddAsync(attachment);

            return new CreateAttachmentResponseModel
            {
                Id = addedAttachmentMap.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var cooperative = await _attachmentRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _attachmentRepository.DeleteAsync(cooperative)).Id
        };
    }

    public async Task<IEnumerable<MappingResponseModel>> GetAllAsync(AttachmentSearchParams attachmentSearchParams)
    {
        var _attachment = await _attachmentRepository.GetAllAsync(c => c.LoanApplicationId == attachmentSearchParams.ApplicationId);

        

        int numberOfObjectsPerPage = attachmentSearchParams.PageSize;

        var queryResultPage = _attachment
            .Skip(numberOfObjectsPerPage * (attachmentSearchParams.PageNumber - 1))
            .Take(numberOfObjectsPerPage);

       

        var list = _mapper.Map<IEnumerable<MappingResponseModel>>(queryResultPage).ToList();
        

        return list;
    }

    public async Task<UpdateAttachmentResponseModel> UpdateAsync(Guid id, UpdateAttachmentModel updateAttachmentModel)
    {
        var attachment = await _attachmentRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateAttachmentModel, attachment);

        return new UpdateAttachmentResponseModel
        {
            Id = (await _attachmentRepository.UpdateAsync(attachment)).Id
        };
    }
}
