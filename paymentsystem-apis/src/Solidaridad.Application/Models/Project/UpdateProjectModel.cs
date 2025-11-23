using Solidaridad.Core.Entities;

namespace Solidaridad.Application.Models.Project;

public class UpdateProjectModel
{
    public string ProjectName { get; set; }
    public Guid CountryId { get; set; }
    public string Description { get; set; }
    
    public string ProjectCode { get; set; }
}

public class UpdateProjectResponseModel : BaseResponseModel { }
