using Backend.Application.Rent.RentMotorcycle.RentMotorcycleSpecification;
using Backend.Domain.Entities;
using Backend.Domain.Repositories.Motorcycle;
using Backend.Domain.Repositories.Rent;
using Moq;

namespace Backend.Tests.Application.Rent.RentMotorcycle.Specifications;

public class MotorcycleSpecificationTests
{
    [Fact]
    public async Task IsSatisfiedBy_ValidMotorcycle_ShouldNotAddErrors()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        var specification = new MotorcycleSpecification(motorcycleRepositoryMock.Object, rentRepositoryMock.Object);

        motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId))
            .ReturnsAsync(new Motorcycle(
                2023, "Honda CBR 600RR", "PAL2024"
            ));

        rentRepositoryMock.Setup(repo => repo.GetByMotorcycleId(motorcycleId))
            .ReturnsAsync((Backend.Domain.Entities.Rent)null);

        var errors = new List<string>();

        // Act
        await specification.IsSatisfiedBy(motorcycleId, errors);

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public async Task IsSatisfiedBy_MotorcycleNotFound_ShouldAddError()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        var specification = new MotorcycleSpecification(motorcycleRepositoryMock.Object, rentRepositoryMock.Object);

        motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId))
            .ReturnsAsync((Motorcycle)null);

        var errors = new List<string>();

        // Act
        await specification.IsSatisfiedBy(motorcycleId, errors);

        // Assert
        Assert.Single(errors);
        Assert.Contains("Motorcycle not found.", errors);
    }

    [Fact]
    public async Task IsSatisfiedBy_MotorcycleRented_ShouldAddError()
    {
        // Arrange
        var motorcycleId = Guid.NewGuid();
        var motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        var specification = new MotorcycleSpecification(motorcycleRepositoryMock.Object, rentRepositoryMock.Object);

        motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId))
            .ReturnsAsync(new Motorcycle(2022, "Kawasaki Ninja ZX-10R", "AZX1204"));

        rentRepositoryMock.Setup(repo => repo.GetByMotorcycleId(motorcycleId))
            .ReturnsAsync(new Backend.Domain.Entities.Rent(Guid.NewGuid(), motorcycleId, new Plan(7, 200)));

        var errors = new List<string>();

        // Act
        await specification.IsSatisfiedBy(motorcycleId, errors);

        // Assert
        Assert.Single(errors);
        Assert.Contains("Motorcycle rented.", errors);
    }
}