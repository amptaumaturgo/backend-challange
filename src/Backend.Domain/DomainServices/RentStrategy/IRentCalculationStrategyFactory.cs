using Backend.Domain.Entities;

namespace Backend.Domain.DomainServices.RentStrategy;

public interface IRentCalculationStrategyFactory
{
    IRentCalculationStrategy GetStrategy(Rent rent, DateTime returnDate);
}