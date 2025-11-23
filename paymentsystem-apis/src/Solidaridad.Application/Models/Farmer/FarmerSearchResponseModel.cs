using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.Farmer;

public class FarmerSearchResponseModel : BaseResponseModel
{
    public List<FarmerResponseModel> Farmers { get; set; }
    public FarmerStatsModel FarmerStats { get; set; } = new FarmerStatsModel();
    public int TotalCount { get; set; }
}
