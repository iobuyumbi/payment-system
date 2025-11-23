
namespace Solidaridad.Application.Models.Country;

public class CountryResponseModel : BaseResponseModel
{

    public string CountryName { get; set; }
    
    public string Code { get; set; }
    
    public bool? IsActive { get; set; }
    
    public string CurrencyName { get; set; }
    
    public string CurrencyPrefix { get; set; }
    
    public string CurrencySuffix { get; set; }

    //public static implicit operator CountryResponseModel(Core.Entities.Country v)
    //{
    //    throw new NotImplementedException();
    //}
}
