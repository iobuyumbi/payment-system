using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.Farmer;

public class FarmerStatsModel
{
    public int TotalFarmers { get; set; }
    public int VerifiedFarmers { get; set; }
    public int UnverifiedFarmers { get; set; }
}
