using Solidaridad.Application.Models.AdminLevels;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solidaridad.Application.Models.DocumentType;

namespace Solidaridad.Application.Services;

public interface IDocumentTypeService
{
    Task<IEnumerable<DocumentTypeResponseModel>> GetAllAsync();



    //Task<CreateCountryResponseModel> CreateAsync(CreateCountryModel createCountryModel);

    //Task<BaseResponseModel> DeleteAsync(Guid id);

    //Task<UpdateCountryResponseModel> UpdateAsync(Guid id, UpdateCountryModel updateCountryModel);
}
