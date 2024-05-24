using Backend.Application.Rent.RentMotorcycle;
using Backend.Application.Rent.RentMotorcycle.RentMotorcycleSpecification;
using Backend.Domain.DomainServices.RentStrategy;
using Backend.Domain.Entities;
using Backend.Domain.Repositories;
using Backend.Shared.Data;
using Moq;

namespace Backend.Tests.Application.Rent.RentMotorcycle;

public class RentMotorcycleCommandHandlerTests
{
    private readonly Mock<ISpecification<Driver>> _driverSpecificationMock;
    private readonly Mock<ISpecification<Motorcycle>> _motorcycleSpecificationMock;
    private readonly Mock<ISpecification<Backend.Domain.Entities.Rent>> _rentSpecificationMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly Mock<IRentCalculationStrategyFactory> _rentCalculationStrategyFactoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly RentMotorcycleCommandHandler _handler;

    public RentMotorcycleCommandHandlerTests()
    {
        _driverSpecificationMock = new Mock<ISpecification<Driver>>();
        _motorcycleSpecificationMock = new Mock<ISpecification<Motorcycle>>();
        _rentSpecificationMock = new Mock<ISpecification<Backend.Domain.Entities.Rent>>();
        _rentRepositoryMock = new Mock<IRentRepository>();
        _rentCalculationStrategyFactoryMock = new Mock<IRentCalculationStrategyFactory>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new RentMotorcycleCommandHandler(
            _driverSpecificationMock.Object,
            _motorcycleSpecificationMock.Object,
            _rentSpecificationMock.Object,
            _rentRepositoryMock.Object,
            _rentCalculationStrategyFactoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_AllSpecificationsSatisfied_ShouldReturnSuccessResponse()
    {
        var plan = new Plan(7, 100);

        // Arrange
        var command = new RentMotorcycleCommand
        {
            DriverId = Guid.NewGuid(),
            MotorcycleId = Guid.NewGuid(),
            PlanId = plan.Id,
            ExpectedDevolutionDate = DateTime.UtcNow.AddDays(7)
        };


        _rentRepositoryMock.Setup(repo => repo.GetPlanById(command.PlanId)).ReturnsAsync(plan);
        _rentCalculationStrategyFactoryMock.Setup(factory =>
                factory.GetStrategy(It.IsAny<Backend.Domain.Entities.Rent>(), command.ExpectedDevolutionDate))
            .Returns(new OnTimeReturnStrategy());

        _driverSpecificationMock.Setup(spec => spec.IsSatisfiedBy(command.DriverId, It.IsAny<List<string>>()))
            .Returns(Task.CompletedTask);
        _motorcycleSpecificationMock.Setup(spec => spec.IsSatisfiedBy(command.MotorcycleId, It.IsAny<List<string>>()))
            .Returns(Task.CompletedTask);
        _rentSpecificationMock.Setup(spec => spec.IsSatisfiedBy(command.PlanId, It.IsAny<List<string>>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Result);
        Assert.Equal(700, ((RentMotorcycleCommandResponse)response.Result).Total);
        Assert.Equal(100, ((RentMotorcycleCommandResponse)response.Result).PricePerDay);
        Assert.Equal(7, ((RentMotorcycleCommandResponse)response.Result).TotalDays);

        _rentRepositoryMock.Verify(repo => repo.Add(It.IsAny<Backend.Domain.Entities.Rent>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }


    [Fact]
    public async Task Handle_SpecificationFails_ShouldReturnErrorResponse()
    {
        // Arrange
        var command = new RentMotorcycleCommand
        {
            DriverId = Guid.NewGuid(),
            MotorcycleId = Guid.NewGuid(),
            PlanId = Guid.NewGuid(),
            ExpectedDevolutionDate = DateTime.UtcNow.AddDays(7)
        };
         
        var errorMessages = new List<string> { "Driver not eligible" };
        _driverSpecificationMock.Setup(spec => spec.IsSatisfiedBy(command.DriverId, It.IsAny<List<string>>()))
            .Callback<Guid, List<string>>((id, errors) => errors.AddRange(errorMessages))
            .Returns(Task.FromResult<object>(null!)); 

        _motorcycleSpecificationMock.Setup(spec => spec.IsSatisfiedBy(command.MotorcycleId, It.IsAny<List<string>>()))
            .Returns(Task.CompletedTask);

        _rentSpecificationMock.Setup(spec => spec.IsSatisfiedBy(command.PlanId, It.IsAny<List<string>>()))
            .Returns(Task.CompletedTask);


        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Contains("Driver not eligible", response.Errors);

        _rentRepositoryMock.Verify(repo => repo.Add(It.IsAny<Backend.Domain.Entities.Rent>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);
    }
}