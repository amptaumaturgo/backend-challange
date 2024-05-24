using Backend.Domain.DomainServices.RentStrategy;
using Backend.Domain.Entities;

namespace Backend.Tests.Domain.RentStrategyTests;

public class OnTimeReturnStrategyTests
{
    [Fact]
    public void CalculateTotal_WithExactExpectedEndDate_ShouldReturnFullPrice()
    {
        // Arrange
        var plan = new Plan(7, 10);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.ExpectedEndDate;
        var strategy = new OnTimeReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(70, total); 
    }

    [Fact]
    public void CalculateTotal_WithEarlyReturn_ShouldReturnFullPrice()
    {
        // Arrange
        var plan = new Plan(7, 10);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.StartDate.AddDays(5); 
        var strategy = new OnTimeReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(70, total); 
    }

    [Fact]
    public void CalculateTotal_WithLateReturn_ShouldReturnFullPrice()
    {
        // Arrange
        var plan = new Plan(7, 10);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.ExpectedEndDate.AddDays(3);  
        var strategy = new OnTimeReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(70, total); 
    }

    [Fact]
    public void CalculateTotal_WithExactExpectedEndDateForDifferentPlan_ShouldReturnFullPrice()
    {
        // Arrange
        var plan = new Plan  (10,  15);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.ExpectedEndDate; 
        var strategy = new OnTimeReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(150, total);
    }
}