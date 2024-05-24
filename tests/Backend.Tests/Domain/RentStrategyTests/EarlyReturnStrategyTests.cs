using Backend.Domain.DomainServices.RentStrategy;
using Backend.Domain.Entities;

namespace Backend.Tests.Domain.RentStrategyTests;

public class EarlyReturnStrategyTests
{
    [Fact]
    public void CalculateTotal_WithNoUnusedDays_ShouldReturnFullPriceWithoutPenalty()
    {
        // Arrange
        var plan = new Plan (7, 10);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.ExpectedEndDate; 
        var strategy = new EarlyReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(70, total);
    }

    [Fact]
    public void CalculateTotal_WithUnusedDays_WeeklyPlan_ShouldApply20PercentPenalty()
    {
        // Arrange
        var plan = new Plan (7, 10);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.StartDate.AddDays(4);  
        var strategy = new EarlyReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert 
        Assert.Equal(76, total); 
    }
      
    [Fact]
    public void CalculateTotal_WithUnusedDays_ExactDaysUsed_ShouldNotApplyPenalty()
    {
        // Arrange
        var plan = new Plan(5, 20);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.ExpectedEndDate;  
        var strategy = new EarlyReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(100, total);  
    }
}