using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Solidaridad.Core.Entities;

public class MasterLoanAppStage: BaseEntity
{
    [StringLength(50)] public string StageText { get; set; }
}
