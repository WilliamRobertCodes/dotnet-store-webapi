using System;
using System.Security.Claims;
using EStoreWebApi.Data;
using EStoreWebApi.Entities;
using EStoreWebApi.Requests.AccountRequests;
using EStoreWebApi.Responses.AccountResponses;
using EStoreWebApi.Services.PasswordHashing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController : Controller
{
	private readonly AppDbContext _db;
    private readonly IPasswordhasher _hasher;

    public AccountsController(AppDbContext db, IPasswordhasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(AuthResponse))]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null || _hasher.VerifyPassword(request.Password, user.PasswordHash) is false)
            return LoginError();

        await AuthenticateUser(user);

        return Ok(AuthResponse.FromUser(user));
    }

    [HttpPost("register")]
    [ProducesResponseType(200, Type = typeof(AuthResponse))]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var userExists = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (userExists is not null)
        {
            ModelState.AddModelError("email", "This email is already in use");
            return UnprocessableEntity(ModelState);
        }

        var newUser = new User()
        {
            Email = request.Email,
            UserName = request.UserName,
            PasswordHash = _hasher.HashPassword(request.Password),
        };

        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();

        await AuthenticateUser(newUser);

        return Ok(AuthResponse.FromUser(newUser));
    }

    [HttpGet("me")]
    [ProducesResponseType(200, Type = typeof(AuthResponse))]
    public async Task<ActionResult<AuthResponse>> Me()
    {
        var claim = HttpContext.User?.FindFirst(c => c.Type == ClaimTypes.Name);

        if (claim is null)
            return Ok(AuthResponse.FromUser(null));

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == claim.Value);

        if (user is null)
            await HttpContext.SignOutAsync();

        return Ok(AuthResponse.FromUser(user));
    }

    private async Task AuthenticateUser(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Email),
        };

        var authProps = new AuthenticationProperties()
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddHours(2),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            authProps);
    }

    [HttpPost("logout")]
    [ProducesResponseType(200, Type = typeof(LogoutResponse))]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return Ok(new LogoutResponse(true));
    }

    private IActionResult LoginError()
    {
        ModelState.AddModelError("email", "These credentials do not match our records");
        return UnprocessableEntity(ModelState);
    }
}

