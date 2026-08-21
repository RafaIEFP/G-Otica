namespace GOtica.Domain.Repositories.Invite;

public interface IInviteWriteOnlyRepository
{
    Task Add(Entities.Invite invite);
}
