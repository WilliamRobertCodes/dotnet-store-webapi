using System;
using System.ComponentModel.DataAnnotations;

namespace EStoreWebApi.Requests.AccountRequests;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}

