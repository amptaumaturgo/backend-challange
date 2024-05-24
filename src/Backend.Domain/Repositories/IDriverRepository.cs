using Backend.Domain.Entities;
using Backend.Shared.Data;

namespace Backend.Domain.Repositories;

public interface IDriverRepository : IRepository<Driver>
{
    Task<Driver?> GetByIdAsync(Guid driverId);
    Task<bool> ExistentCnpj(string cnpj);
    void Add(Driver driver);
    void Update(Driver driver);
    Task<Driver?> GetByUserId(Guid userId);
}