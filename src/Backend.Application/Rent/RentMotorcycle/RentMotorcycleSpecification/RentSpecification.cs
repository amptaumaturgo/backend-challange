using Backend.Domain.Repositories.Rent;

namespace Backend.Application.Rent.RentMotorcycle.RentMotorcycleSpecification;

public class RentSpecification(IRentRepository rentRepository) : ISpecification<Domain.Entities.Rent>
{
    public async Task IsSatisfiedBy(Guid entityId, List<string> errors)
    {
        var plan = await rentRepository.GetPlanById(entityId);
        if (plan == null)
        {
            errors.Add("Plan not found.");
            return;
        }
    }
}