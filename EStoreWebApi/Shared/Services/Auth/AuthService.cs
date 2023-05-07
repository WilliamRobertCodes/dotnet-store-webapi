using System.Security.Claims;
using EStoreWebApi.Data;
using EStoreWebApi.Features.Accounts.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Shared.Services.Auth;

public class AuthService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly AppDbContext _db;

    public AuthService(IHttpContextAccessor contextAccessor, AppDbContext db)
    {
        _db = db;
        _contextAccessor = contextAccessor;
    }

    public async Task<User?> UserAsync()
    {
        var context = _contextAccessor.HttpContext;

        var claim = context.User?.FindFirst(c => c.Type == ClaimTypes.Name);

        if (claim is null)
            return null;

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == claim.Value);

        if (user is null)
            await context.SignOutAsync();

        return user;
    }
    
    public async Task<User> RequiredUserAsync()
    {
        var context = _contextAccessor.HttpContext;

        var claim = context.User?.FindFirst(c => c.Type == ClaimTypes.Name);

        if (claim is null)
            throw new ArgumentNullException(nameof(claim));

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == claim.Value);

        if (user is null)
        {
            await context.SignOutAsync();
            throw new ArgumentNullException(nameof(user));
        }

        return user;
    }
}

