namespace EStoreWebApi.Shared.Services.PasswordHashing;

public class BCryptPasswordHasher : IPasswordhasher
{
    public string HashPassword(string plain)
        => BCrypt.Net.BCrypt.HashPassword(plain);

    public bool VerifyPassword(string plain, string hash)
        => BCrypt.Net.BCrypt.Verify(plain, hash);
}

