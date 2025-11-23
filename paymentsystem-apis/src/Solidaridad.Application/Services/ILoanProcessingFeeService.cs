using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanProcessingFee;

namespace Solidaridad.Application.Services;

public interface ILoanProcessingFeeService
{
    Task<CreateLoanProcessingFeeResponseModel> CreateAsync(CreateLoanProcessingFeeModel project);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<LoanProcessingFeeResponseModel>> GetAllAsync();

    Task<UpdateLoanProcessingFeeResponseModel> UpdateAsync(Guid id, UpdateLoanProcessingFeeModel projectModel);
}
