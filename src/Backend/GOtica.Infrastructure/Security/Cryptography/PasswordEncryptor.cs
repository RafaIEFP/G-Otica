using GOtica.Domain.Security.Cryptography;

namespace GOtica.Infrastructure.Security.Cryptography;

internal sealed class PasswordEncryptor : IPasswordEncryptor
{
    public string Encrypt(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool IsValid(string password, string passwordHash) =>  BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
