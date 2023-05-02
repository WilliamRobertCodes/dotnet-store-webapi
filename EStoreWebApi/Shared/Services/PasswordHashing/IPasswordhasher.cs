namespace EStoreWebApi.Shared.Services.PasswordHashing;

public interface IPasswordhasher
{
    public string HashPassword(string plain);

    public bool VerifyPassword(string plain, string hash);
}

