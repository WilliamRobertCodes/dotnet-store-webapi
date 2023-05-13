using EStoreWebApi.Shared.Entities;
using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Accounts.Entities;

public class UserAddress : BaseEntity
{
    public int UserId { get; set; }

    public User User { get; set; }

    public bool IsFavoriteAddress { get; set; } = false;
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? CompanyName { get; set; }

    public string Street1 { get; set; }

    public string? Street2 { get; set; }

    public string City { get; set; }

    public string ZipCode { get; set; }

    public int CountryId { get; set; }

    public Country Country { get; set; }
}
