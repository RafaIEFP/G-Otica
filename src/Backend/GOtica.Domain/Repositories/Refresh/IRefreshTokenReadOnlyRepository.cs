namespace GOtica.Domain.Repositories.Refresh;

public interface IRefreshTokenReadOnlyRepository
{
    Task<Entities.RefreshToken?> Get(string token);

    Task<bool> HasRefresTokenAssociated(Guid userId, Guid accessTokenIdentifier);
}
