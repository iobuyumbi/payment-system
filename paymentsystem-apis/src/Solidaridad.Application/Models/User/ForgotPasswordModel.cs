using System.ComponentModel.DataAnnotations;

namespace Solidaridad.Application.Models.User;

public class ForgotPasswordModel
{
    [Required]
    public string Email { get; set; }
}
