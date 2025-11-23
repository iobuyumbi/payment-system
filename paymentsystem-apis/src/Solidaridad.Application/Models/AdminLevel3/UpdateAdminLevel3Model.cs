using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Models.Ward;

public class UpdateAdminLevel3Model
{
    public string WardName { get; set; }
    public string WardCode { get; set; }
    public Guid SubCountyId { get; set; }

    public bool IsActive { get; set; } = true;
}
public class UpdateAdminLevel3ResponseModel : BaseResponseModel { }
