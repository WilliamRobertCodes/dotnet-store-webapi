using EStoreWebApi.Data;
using EStoreWebApi.Extensions;
using EStoreWebApi.Features.Accounts.Entities;
using EStoreWebApi.Features.Accounts.Requests;
using EStoreWebApi.Features.Accounts.Responses;
using EStoreWebApi.Shared.Responses;
using EStoreWebApi.Shared.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Features.Accounts.Controllers;

[ApiController]
[Route("api/accounts/addresses")]
public class AddressesController : Controller
{
    private readonly AppDbContext _db;
    private readonly AuthService _auth;

    public AddressesController(AppDbContext db, AuthService auth)
    {
        _db = db;
        _auth = auth;
    }
    
    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddressResponse>))]
    public async Task<IActionResult> ListAddresses()
    {
        var user = await _auth.RequiredUserAsync();
        
        var addresses = await _db.UserAddresses
            .AsNoTracking()
            .Include(a => a.Country)
            .Where(a => a.UserId == user.Id)
            .ToListAsync();

        return Ok(addresses.Select(AddressResponse.FromAddress));
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressResponse))]
    public async Task<IActionResult> CreateAddress(CreateOrUpdateAddressRequest request)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ErrorResponse.Make(ModelState));
        }

        var user = await _auth.RequiredUserAsync();
        await _db.Entry(user).Collection(u => u.UserAddresses).LoadAsync();
        var country = await _db.Countries.FindAsync(request.CountryId);

        if (country is null)
        {
            ModelState.AddModelError("CountryId", $"Country with id {request.CountryId} does not exist.");
            return UnprocessableEntity(ErrorResponse.Make(ModelState));
        }

        if (user.FirstName is null)
        {
            user.FirstName = request.FirstName;
        }
        
        if (user.LastName is null)
        {
            user.LastName = request.LastName;
        }
        
        var address = new UserAddress()
        {
            IsFavoriteAddress = user.UserAddresses.IsEmpty(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CompanyName = request.CompanyName,
            Street1 = request.Street1,
            Street2 = request.Street2,
            City = request.City,
            ZipCode = request.ZipCode,
            Country = country,
            User = user,
        };

        await _db.UserAddresses.AddAsync(address);
        await _db.SaveChangesAsync();

        return Ok(AddressResponse.FromAddress(address));
    }

    [Authorize]
    [HttpPut("{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressResponse))]
    public async Task<IActionResult> UpdateAddress(int addressId, CreateOrUpdateAddressRequest request)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ErrorResponse.Make(ModelState));
        }

        var user = await _auth.RequiredUserAsync();
        await _db.Entry(user).Collection(u => u.UserAddresses).LoadAsync();
        var address = user.UserAddresses.FirstOrDefault(a => a.Id == addressId);

        if (address is null)
        {
            return NotFound();
        }
        
        var country = await _db.Countries.FindAsync(request.CountryId);

        if (country is null)
        {
            ModelState.AddModelError("CountryId", $"Country with id {request.CountryId} does not exist.");
            return UnprocessableEntity(ErrorResponse.Make(ModelState));
        }
        
        address.FirstName = request.FirstName;
        address.LastName = request.LastName;
        address.CompanyName = request.CompanyName;
        address.Street1 = request.Street1;
        address.Street2 = request.Street2;
        address.City = request.City;
        address.ZipCode = request.ZipCode;
        address.CountryId = request.CountryId;

        await _db.SaveChangesAsync();

        return Ok(AddressResponse.FromAddress(address));
    }

    [Authorize]
    [HttpDelete("{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAddress(int addressId)
    {
        var user = await _auth.RequiredUserAsync();
        var address = await _db.UserAddresses
            .Include(a => a.Country)
            .Where(a => a.UserId == user.Id)
            .Where(a => a.Id == addressId)
            .FirstOrDefaultAsync();

        if (address is null)
            return NotFound();

        _db.UserAddresses.Remove(address);
        await _db.SaveChangesAsync();
        
        return Ok();
    }

    [Authorize]
    [HttpPut("{addressId}/favorite")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressResponse))]
    public async Task<IActionResult> FavoriteAddress(int addressId)
    {
        var user = await _auth.RequiredUserAsync();
        await _db.Entry(user).Collection(u => u.UserAddresses).LoadAsync();
        var address = user.UserAddresses.Find(a => a.Id == addressId);
        
        if (address is null)
            return NotFound();

        user.UserAddresses.ForEach(a => a.IsFavoriteAddress = false);
        address.IsFavoriteAddress = true;

        await _db.SaveChangesAsync();

        return Ok(AddressResponse.FromAddress(address));
    }
}