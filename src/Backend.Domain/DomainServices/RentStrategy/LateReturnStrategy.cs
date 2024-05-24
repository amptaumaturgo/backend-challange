using Backend.Domain.Entities;

namespace Backend.Domain.DomainServices.RentStrategy;

public class LateReturnStrategy : IRentCalculationStrategy
{
    public decimal CalculateTotal(Rent rent, DateTime expectedDevolutionDate)
    {
        var extraDays = (expectedDevolutionDate - rent.ExpectedEndDate).Days;
        decimal extraCharge = 0;
        if (extraDays > 0)
        {
              extraCharge = extraDays * 50;
        }
        return (rent.Plan.Days * rent.Plan.PricePerDay) + extraCharge;
    }
}

