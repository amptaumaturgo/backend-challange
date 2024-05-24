using Backend.Domain.Entities;

namespace Backend.Domain.DomainServices.RentStrategy;

public interface IRentCalculationStrategy
{
    decimal CalculateTotal(Rent rent, DateTime expectedDevolutionDate); 
}