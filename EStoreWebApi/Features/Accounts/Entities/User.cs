using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Accounts.Entities;

public class User : TimestampedEntity
{
    public string Email { get; set; }

    public string PasswordHash { get; set; }
}