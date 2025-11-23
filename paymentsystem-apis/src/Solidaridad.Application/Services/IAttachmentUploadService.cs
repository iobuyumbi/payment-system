using Solidaridad.Application.Models.LoanAttachmentModel;
using Solidaridad.Application.Models;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Application.Models.AttachmentUpload;

namespace Solidaridad.Application.Services;

public interface IAttachmentUploadService
{
    Task<CreateAttachmentUploadResponseModel> CreateAsync(CreateAttachmentUploadModel createAttachmentModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<AttachmentResponseModel>> GetAllAsync(AttachmentSearchParams attachmentSearchParams);

    Task<UpdateAttachmentUploadResponseModel> UpdateAsync(Guid id, UpdateAttachmentUploadModel updateAttachmentModel);
}
