namespace Solidaridad.Application.Models.Ward;

public class WardResponseModel : BaseResponseModel
{
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public Guid ConstituencyId { get; set; }
}
