using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.AttachmentUpload;
using Solidaridad.Application.Models.LoanAttachmentModel;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;

namespace Solidaridad.Application.Services.Impl;

public class AttachmentUploadService : IAttachmentUploadService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly IAttachmentUploadRepository _attachmentRepository;


    public AttachmentUploadService(IMapper mapper, IAttachmentUploadRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
        _mapper = mapper;

    }
    #endregion
    public async Task<CreateAttachmentUploadResponseModel> CreateAsync(CreateAttachmentUploadModel createAttachmentModel)
    {
        try
        {

            var attachment = _mapper.Map<AttachmentFile>(createAttachmentModel);
            attachment.Id = Guid.NewGuid();
            var addedAttachmentMap = await _attachmentRepository.AddAsync(attachment);

            return new CreateAttachmentUploadResponseModel
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

    public async Task<IEnumerable<AttachmentResponseModel>> GetAllAsync(AttachmentSearchParams attachmentSearchParams)
    {
        var _attachment = await _attachmentRepository.GetAllAsync(c =>1==1);



        int numberOfObjectsPerPage = attachmentSearchParams.PageSize;

        var queryResultPage = _attachment
            .Skip(numberOfObjectsPerPage * (attachmentSearchParams.PageNumber - 1))
            .Take(numberOfObjectsPerPage);



        var list = _mapper.Map<IEnumerable<AttachmentResponseModel>>(queryResultPage).ToList();


        return list;
    }

    public async Task<UpdateAttachmentUploadResponseModel> UpdateAsync(Guid id, UpdateAttachmentUploadModel updateAttachmentModel)
    {
        var attachment = await _attachmentRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateAttachmentModel, attachment);

        return new UpdateAttachmentUploadResponseModel
        {
            Id = (await _attachmentRepository.UpdateAsync(attachment)).Id
        };
    }
}
