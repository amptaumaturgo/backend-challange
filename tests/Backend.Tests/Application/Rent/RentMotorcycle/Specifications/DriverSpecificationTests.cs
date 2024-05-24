using Backend.Application.Rent.RentMotorcycle.RentMotorcycleSpecification;
using Backend.Domain.Entities.Enums;
using Backend.Domain.Entities;
using Backend.Domain.Repositories;
using Moq;

namespace Backend.Tests.Application.Rent.RentMotorcycle.Specifications;

public class DriverSpecificationTests
{
    [Fact]
    public async Task IsSatisfiedBy_ValidDriver_ShouldNotAddErrors()
    {
        // Arrange
        var driverId = Guid.NewGuid();
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        var specification = new DriverSpecification(driverRepositoryMock.Object, rentRepositoryMock.Object);

        driverRepositoryMock.Setup(repo => repo.GetByIdAsync(driverId))
            .ReturnsAsync(new Driver("Alexandre Magno", "12345678901234", "ABC123456", new DateTime(1990, 5, 15), DriverLicenseType.A, driverId));


        rentRepositoryMock.Setup(repo => repo.GetRentByDriverId(driverId))
                          .ReturnsAsync((Backend.Domain.Entities.Rent)null!);

        var errors = new List<string>();

        // Act
        await specification.IsSatisfiedBy(driverId, errors);

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public async Task IsSatisfiedBy_DriverNotFound_ShouldAddError()
    {
        // Arrange
        var driverId = Guid.NewGuid();
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        var specification = new DriverSpecification(driverRepositoryMock.Object, rentRepositoryMock.Object);

        driverRepositoryMock.Setup(repo => repo.GetByIdAsync(driverId))
                            .ReturnsAsync((Driver)null!);

        var errors = new List<string>();

        // Act
        await specification.IsSatisfiedBy(driverId, errors);

        // Assert
        Assert.Single(errors);
        Assert.Contains("Driver not found.", errors);
    }

    [Fact]
    public async Task IsSatisfiedBy_InvalidDriverLicenseType_ShouldAddError()
    {
        // Arrange
        var driverId = Guid.NewGuid();
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        var specification = new DriverSpecification(driverRepositoryMock.Object, rentRepositoryMock.Object);

        driverRepositoryMock.Setup(repo => repo.GetByIdAsync(driverId))
            .ReturnsAsync(new Driver("Alexandre Magno", "12345678901234", "ABC123456", new DateTime(1990, 5, 15), DriverLicenseType.B, driverId));


        var errors = new List<string>();

        // Act
        await specification.IsSatisfiedBy(driverId, errors);

        // Assert
        Assert.Single(errors);
        Assert.Contains("Invalid driver license type.", errors);
    }

    [Fact]
    public async Task IsSatisfiedBy_ActiveRent_ShouldAddError()
    {
        // Arrange
        var driverId = Guid.NewGuid();
        var driverRepositoryMock = new Mock<IDriverRepository>();
        var rentRepositoryMock = new Mock<IRentRepository>();
        var specification = new DriverSpecification(driverRepositoryMock.Object, rentRepositoryMock.Object);

        driverRepositoryMock.Setup(repo => repo.GetByIdAsync(driverId))
            .ReturnsAsync(new Driver("Alexandre Magno", "12345678901234", "ABC123456", new DateTime(1990, 5, 15), DriverLicenseType.A, driverId));

        rentRepositoryMock.Setup(repo => repo.GetRentByDriverId(driverId))
                          .ReturnsAsync(new Backend.Domain.Entities.Rent(driverId, Guid.NewGuid(), new Plan(7, 20)));

        var errors = new List<string>();

        // Act
        await specification.IsSatisfiedBy(driverId, errors);

        // Assert
        Assert.Single(errors);
        Assert.Contains("Driver has active rent.", errors);
    }
}