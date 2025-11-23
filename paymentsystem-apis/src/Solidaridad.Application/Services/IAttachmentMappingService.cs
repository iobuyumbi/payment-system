using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanAttachmentModel;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IAttachmentMappingService 
{
    Task<CreateAttachmentResponseModel> CreateAsync(CreateAttachmentModel createAttachmentModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<MappingResponseModel>> GetAllAsync(AttachmentSearchParams attachmentSearchParams);
    
    Task<UpdateAttachmentResponseModel> UpdateAsync(Guid id, UpdateAttachmentModel updateAttachmentModel);
}
