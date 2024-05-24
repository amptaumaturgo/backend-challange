using Backend.Domain.Entities;

namespace Backend.Domain.DomainServices.RentStrategy;

public class EarlyReturnStrategy : IRentCalculationStrategy
{
    public decimal CalculateTotal(Rent rent, DateTime expectedDevolutionDate)
    {
        int daysUsed = (expectedDevolutionDate - rent.StartDate).Days;
        int unusedDays = rent.Plan.Days - daysUsed;

        decimal dailyRate = rent.Plan.PricePerDay;
        decimal penalty = 0;

        if (unusedDays > 0)
        {
            penalty = CalculatePercentage(unusedDays * dailyRate, rent.Plan.Days == 7 ? 20 : 40);
        }

        return (rent.Plan.Days * rent.Plan.PricePerDay) + penalty;
    }

    private decimal CalculatePercentage(decimal value, decimal percentage)
    {
        return (value * percentage) / 100;
    }
}