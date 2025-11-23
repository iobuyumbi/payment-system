using AutoMapper;
using Solidaridad.Application.Models.DocumentType;
using Solidaridad.Application.Models.PaymentDeductible;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class DocumentTypeService : IDocumentTypeService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly IDocumentTypeRepository _documentTypeRepository;


    public DocumentTypeService(IMapper mapper,IDocumentTypeRepository documentTypeRepository
        )
    {
        _mapper = mapper;
        _documentTypeRepository = documentTypeRepository;
      
    }
    #endregion

    #region Methods
    public async  Task<IEnumerable<DocumentTypeResponseModel>> GetAllAsync()
    {
        var documentType = await _documentTypeRepository.GetAllAsync(ti =>true);
        return _mapper.Map<IEnumerable<DocumentTypeResponseModel>>(documentType);
    }
    #endregion
}
