namespace GOtica.Domain.Security.Cryptography;

public interface IPasswordEncryptor
{
    string Encrypt(string password);
    bool IsValid(string password, string passwordHash);
}
