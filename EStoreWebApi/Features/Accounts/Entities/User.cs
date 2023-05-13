using System.ComponentModel.DataAnnotations.Schema;
using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Accounts.Entities;

public class User : TimestampedEntity
{
    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    
    public List<UserAddress> UserAddresses { get; set; }

    [NotMapped]
    public bool InfosCompleted =>
        FirstName is not null && LastName is not null;
}