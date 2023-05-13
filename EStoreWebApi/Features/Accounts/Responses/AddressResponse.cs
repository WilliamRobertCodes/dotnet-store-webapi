using EStoreWebApi.Features.Accounts.Entities;
using EStoreWebApi.Shared.Responses;

namespace EStoreWebApi.Features.Accounts.Responses;

public class AddressResponse
{
    public int Id { get; set; }
    public bool IsFavoriteAddress { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? CompanyName { get; set; }
    public string Street1 { get; set; }
    public string? Street2 { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public int CountryId { get; set; }
    public CountryResponse Country { get; set; }

    public static AddressResponse FromAddress(UserAddress address) => new()
    {
        Id = address.Id,
        IsFavoriteAddress = address.IsFavoriteAddress,
        FirstName = address.FirstName,
        LastName = address.LastName,
        CompanyName = address.CompanyName,
        Street1 = address.Street1,
        Street2 = address.Street2,
        City = address.City,
        ZipCode = address.ZipCode,
        CountryId = address.Country.Id,
        Country = CountryResponse.FromCountry(address.Country),
    };
}