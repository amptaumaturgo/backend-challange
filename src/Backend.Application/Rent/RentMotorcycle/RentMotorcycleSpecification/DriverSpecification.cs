using Backend.Domain.Entities.Enums;
using Backend.Domain.Repositories;

namespace Backend.Application.Rent.RentMotorcycle.RentMotorcycleSpecification;

public class DriverSpecification(IDriverRepository driverRepository, IRentRepository rentRepository) : ISpecification<Domain.Entities.Driver>
{
    public async Task IsSatisfiedBy(Guid entityId, List<string> errors)
    {
        var driver = await driverRepository.GetByIdAsync(entityId);
        if (driver is null)
        {
            errors.Add("Driver not found.");
            return;
        }

        var validDriverLicenseType = driver.DriverLicenseType is DriverLicenseType.A or DriverLicenseType.Ab;

        if (!validDriverLicenseType)
        {
            errors.Add("Invalid driver license type.");
        }

        var rent = await rentRepository.GetRentByDriverId(entityId);
        if (rent is not null)
        {
            errors.Add("Driver has active rent.");
        }
    }
}