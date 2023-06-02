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
    
    private User? User { get; set; }

    public AuthService(IHttpContextAccessor contextAccessor, AppDbContext db)
    {
        _db = db;
        _contextAccessor = contextAccessor;
    }

    public async Task<User?> UserAsync()
    {
        if (User is not null)
            return User;

        var context = _contextAccessor.HttpContext;

        var claim = context.User?.FindFirst(c => c.Type == ClaimTypes.Name);

        if (claim is null)
            return null;

        User = await _db.Users.FirstOrDefaultAsync(u => u.Email == claim.Value);

        if (User is null)
            await context.SignOutAsync();

        return User;
    }
    
    public async Task<User> RequiredUserAsync()
    {
        if (User is not null)
            return User;
        
        var context = _contextAccessor.HttpContext;

        var claim = context.User?.FindFirst(c => c.Type == ClaimTypes.Name);

        if (claim is null)
            throw new ArgumentNullException(nameof(claim));

        User = await _db.Users.FirstOrDefaultAsync(u => u.Email == claim.Value);

        if (User is null)
        {
            await context.SignOutAsync();
            throw new ArgumentNullException(nameof(User));
        }

        return User;
    }
}

