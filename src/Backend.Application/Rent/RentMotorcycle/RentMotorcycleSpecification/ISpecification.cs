namespace Backend.Application.Rent.RentMotorcycle.RentMotorcycleSpecification;

public interface ISpecification<in T> where T : class
{
    Task IsSatisfiedBy(Guid entityId, List<string> errors);
}