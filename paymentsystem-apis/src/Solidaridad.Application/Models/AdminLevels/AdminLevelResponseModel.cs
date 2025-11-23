using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Application.Models.Village;
using Solidaridad.Application.Models.Ward;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Models.AdminLevels;

public class AdminLevelResponseModel
{
    public CountryResponseModel Country { get; set; }
    public ReadOnlyCollection<AdminLevel1ResponseModel> AdminLevel1 { get; set; }
    public ReadOnlyCollection<AdminLevel2ResponseModel> AdminLevel2 { get; set; }
    public ReadOnlyCollection<AdminLevel3ResponseModel> AdminLevel3 { get; set; }
   
}
