namespace Solidaridad.Application.Models.PaymentBatch.Transitions;

public class TransitionRequest
{
    public string Action { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
}

public class TransitionResponse
{
    public string EffectiveRole { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<ActionOption> NextActions { get; set; } = new();
    public bool IsSelfAction { get; set; } = false;
}

public class ActionOption
{
    public string Action { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public bool RestrictedForMaker { get; set; } = false;
}
