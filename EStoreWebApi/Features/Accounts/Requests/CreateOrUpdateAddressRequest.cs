using System.ComponentModel.DataAnnotations;

namespace EStoreWebApi.Features.Accounts.Requests;

public class CreateOrUpdateAddressRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
    
    public string? CompanyName { get; set; }
    
    [Required]
    public string Street1 { get; set; }
    
    public string? Street2 { get; set; }
    
    [Required]
    public string City { get; set; }

    [Required]
    public string ZipCode { get; set; }
    
    [Required]
    public int CountryId { get; set; }
}
