using Backend.Domain.Entities;
using Backend.Shared.Data;

namespace Backend.Domain.Repositories.Driver;

public interface IDriverRepository : IRepository<Entities.Driver>
{
    Task<Entities.Driver?> GetByIdAsync(Guid driverId);
    Task<bool> ExistentCnpj(string cnpj);
    void Add(Entities.Driver driver);
    void Update(Entities.Driver driver);
    Task<Entities.Driver?> GetByUserId(Guid userId);
}