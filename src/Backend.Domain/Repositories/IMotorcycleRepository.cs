using Backend.Domain.Entities;

namespace Backend.Domain.Repositories;

public interface IMotorcycleRepository
{
    void Add(Motorcycle motorcycle);
    void Update(Motorcycle motorcycle);
    void Remove(Motorcycle motorcycle);

    Task<Motorcycle?> GetByIdAsync(Guid id);
    Task<Motorcycle?> GetByPlate(string plate);
    Task<IEnumerable<Motorcycle>> GetAllAsync();

}