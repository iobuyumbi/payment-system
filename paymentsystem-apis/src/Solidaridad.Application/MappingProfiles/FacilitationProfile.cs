using AutoMapper;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.Facilitation;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.Application.MappingProfiles;

public class FacilitationProfile : Profile
{
    public FacilitationProfile()
    {
        CreateMap<PaymentRequestFacilitation, FacilitationResponseModel>();

        CreateMap<PaymentRequestFacilitation, PaymentRequestFacilitationModel>();

        CreateMap<CreateFacilitationModel, PaymentRequestFacilitation>();

        CreateMap<FacilitationResponseModel, PaymentRequestFacilitation>();

        CreateMap<UpdateFacilitationModel, PaymentRequestFacilitation>();

        CreateMap<PaymentRequestFacilitationModel, PaymentRequestFacilitation>();
    }
}
