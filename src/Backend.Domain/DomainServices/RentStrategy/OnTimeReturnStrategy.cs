using Backend.Domain.Entities;

namespace Backend.Domain.DomainServices.RentStrategy;

public class OnTimeReturnStrategy : IRentCalculationStrategy
{
    public decimal CalculateTotal(Rent rent, DateTime expectedDevolutionDate)
    {
        return rent.Plan.Days * rent.Plan.PricePerDay;
    }
}