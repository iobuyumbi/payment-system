using AutoMapper;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class ExcelImportProfile : Profile
{
    public ExcelImportProfile() 
    {
        CreateMap<ExcelImport, ExcelImportResponseModel>();

        CreateMap <CreateExcelImportModel, ExcelImport> ();
    }
}
