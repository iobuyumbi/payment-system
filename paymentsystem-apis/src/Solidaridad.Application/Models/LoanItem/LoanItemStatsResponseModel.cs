namespace Solidaridad.Application.Models.LoanItem;

public class LoanItemStatsResponseModel
{
    public int TotalItemsLoaned { get; set; }
    public float TotalItemValue { get; set; }
    public float TotalFeeCharged { get; set; }
    public float PrincipleAmount { get; set; }
    public float EffectiveBalance { get; set; }
    public float InterestEarned { get; set; }
    public float EffectiveLoanBalance { get; set; }

}
