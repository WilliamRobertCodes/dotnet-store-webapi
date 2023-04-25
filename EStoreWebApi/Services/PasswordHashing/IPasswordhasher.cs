using System;
namespace EStoreWebApi.Services.PasswordHashing;

public interface IPasswordhasher
{
    public string HashPassword(string plain);

    public bool VerifyPassword(string plain, string hash);
}

