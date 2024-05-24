namespace Backend.Shared.Data;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
    bool Commit();
}