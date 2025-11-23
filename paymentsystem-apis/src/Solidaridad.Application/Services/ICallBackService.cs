using Solidaridad.Application.Models;
using Solidaridad.Application.Models.AdminLevels;
using Solidaridad.Application.Models.CallBack;
using Solidaridad.Application.Models.Country;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface ICallBackService
{
    Task<IEnumerable<CallBackResponseModel>> GetAllAsync();

    Task<CreateCallBackResponseModel> CreateAsync(CreateCallBackModel createCallBackModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    //Task<UpdateCountryResponseModel> UpdateAsync(Guid id, UpdateCountryModel updateCountryModel);
}
