using Backend.Domain.Repositories;

namespace Backend.Application.Rent.RentMotorcycle.RentMotorcycleSpecification;

public class MotorcycleSpecification(IMotorcycleRepository motorcycleRepository, IRentRepository rentRepository) : ISpecification<Domain.Entities.Motorcycle>
{
    public async Task IsSatisfiedBy(Guid entityId, List<string> errors)
    {
        var motorcycle = await motorcycleRepository.GetByIdAsync(entityId);
        if (motorcycle is null)
        {
            errors.Add("Motorcycle not found.");
            return;
        }

        var isRented = await rentRepository.GetByMotorcycleId(entityId);
        if (isRented is not null)
        {
            errors.Add("Motorcycle rented."); 
        }
    }
}