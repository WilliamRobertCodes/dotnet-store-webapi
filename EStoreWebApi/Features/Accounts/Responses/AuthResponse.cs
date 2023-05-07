using EStoreWebApi.Features.Accounts.Entities;

namespace EStoreWebApi.Features.Accounts.Responses;


public class AuthResponse
{
    public class AuthUserResponse
    {
        public int Id { get; set; }

        public string Email { get; set; }
    }

    public AuthUserResponse? User { get; set; }

    public static AuthResponse FromUser(User? user)
    {
        if (user is null)
            return new AuthResponse();

        return new AuthResponse()
        {
            User = new()
            {
                Id = user.Id,
                Email = user.Email,
            }
        };
    }
}

