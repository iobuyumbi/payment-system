namespace Solidaridad.Application.Models.Farmer;

public class ModuleResponseModel : BaseResponseModel
{
    public string Name { get; set; }
    public int? ParentId { get; set; }
}
