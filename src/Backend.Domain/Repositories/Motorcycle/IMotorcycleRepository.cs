using Backend.Domain.Entities;

namespace Backend.Domain.Repositories.Motorcycle;

public interface IMotorcycleRepository
{
    void Add(Entities.Motorcycle motorcycle);
    void Update(Entities.Motorcycle motorcycle);
    void Remove(Entities.Motorcycle motorcycle);

    Task<Entities.Motorcycle?> GetByIdAsync(Guid id);
    Task<Entities.Motorcycle?> GetByPlate(string plate);
    Task<IEnumerable<Entities.Motorcycle>> GetAllAsync();

}