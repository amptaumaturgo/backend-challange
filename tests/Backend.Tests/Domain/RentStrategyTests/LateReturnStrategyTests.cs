using Backend.Domain.DomainServices.RentStrategy;
using Backend.Domain.Entities;

namespace Backend.Tests.Domain.RentStrategyTests;

public class LateReturnStrategyTests
{
    [Fact]
    public void CalculateTotal_WithNoLateDays_ShouldReturnFullPriceWithoutExtraCharge()
    {
        // Arrange
        var plan = new Plan(7, 10);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.ExpectedEndDate; // Exact end date
        var strategy = new LateReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(70, total);
    }

    [Fact]
    public void CalculateTotal_WithLateDays_ShouldApplyExtraCharge()
    {
        // Arrange
        var plan = new Plan(7, 10);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.ExpectedEndDate.AddDays(3);
        var strategy = new LateReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(70 + 150, total);
    }

    [Fact]
    public void CalculateTotal_WithNegativeLateDays_ShouldNotApplyExtraCharge()
    {
        // Arrange
        var plan = new Plan(7, 10);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.StartDate.AddDays(5);
        var strategy = new LateReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(70, total);
    }

    [Fact]
    public void CalculateTotal_WithExactExpectedEndDate_ShouldReturnFullPriceWithoutExtraCharge()
    {
        // Arrange
        var plan = new Plan(10, 15);
        var rent = new Rent(Guid.NewGuid(), Guid.NewGuid(), plan);
        var expectedDevolutionDate = rent.ExpectedEndDate; // Exact end date
        var strategy = new LateReturnStrategy();

        // Act
        var total = strategy.CalculateTotal(rent, expectedDevolutionDate);

        // Assert
        Assert.Equal(150, total); // No extra charge, exact end date
    }
}