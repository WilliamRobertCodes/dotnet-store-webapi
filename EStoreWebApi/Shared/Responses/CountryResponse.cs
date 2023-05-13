using EStoreWebApi.Shared.Entities;

namespace EStoreWebApi.Shared.Responses;

public class CountryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }

    public static CountryResponse FromCountry(Country country) => new()
    {
        Id = country.Id,
        Name = country.Name,
        Code = country.Code
    };
}