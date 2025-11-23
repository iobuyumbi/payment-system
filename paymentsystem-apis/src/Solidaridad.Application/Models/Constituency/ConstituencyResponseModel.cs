namespace Solidaridad.Application.Models.Constituency;

public class ConstituencyResponseModel : BaseResponseModel
{
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public Guid CountyId { get; set; }
}
