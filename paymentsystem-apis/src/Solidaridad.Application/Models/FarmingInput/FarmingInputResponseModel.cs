using Solidaridad.Core.Entities;

namespace Solidaridad.Application.Models.FarmingInput;

public class FarmingInputResponseModel : BaseResponseModel
{
    public string InputName { get; set; }
    public InputCategory InputCategory { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
