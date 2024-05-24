using Backend.Domain.Entities;

namespace Backend.Domain.DomainServices.RentStrategy;

public class RentCalculationStrategyFactory: IRentCalculationStrategyFactory
{ 
    public IRentCalculationStrategy GetStrategy(Rent rent, DateTime returnDate)
    {
        if (returnDate < rent.ExpectedEndDate)
        {
            return new EarlyReturnStrategy();
        }
        else if (returnDate > rent.ExpectedEndDate)
        {
            return new LateReturnStrategy();
        }
        else
        {
            return new OnTimeReturnStrategy();
        }
    }
}