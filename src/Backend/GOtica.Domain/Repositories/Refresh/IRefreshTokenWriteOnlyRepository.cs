namespace GOtica.Domain.Repositories.Refresh;

public interface IRefreshTokenWriteOnlyRepository
{
    Task Add(Entities.RefreshToken refreshToken);
    Task DeleteUserRefresh(Guid userId);
}
