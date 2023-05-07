using System.ComponentModel.DataAnnotations;

namespace EStoreWebApi.Features.Accounts.Requests;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Your password must be 8 characters long or more.")]
    public string Password { get; set; }
}
