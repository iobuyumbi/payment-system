using Solidaridad.Core.Entities;

namespace Solidaridad.Application.Models.Project;

public class ProjectResponseModel : BaseResponseModel 
{
    public string ProjectName { get; set; }
    public Guid CountryId { get; set; }
    public string CountryName { get; set; }
   
    public string ProjectCode { get; set; }
    public string Description { get; set; }
}
