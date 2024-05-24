using Backend.Domain.Repositories;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Commands;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Driver.Commands.SaveDriverLicenseImage;

public class SaveDriverLicenseImageCommand : Command
{
    public Guid DriverId { get; set; }
    public IFormFile File { get; set; }
}

public class SaveDriverLicenseImageCommandHandler(IDriverRepository driverRepository) : CommandHandler<SaveDriverLicenseImageCommand>
{
    public override async Task<CommandResponse> Handle(SaveDriverLicenseImageCommand request, CancellationToken cancellationToken)
    {
        var driver = await driverRepository.GetByIdAsync(request.DriverId);

        if (driver is null)
        {
            return "Driver not found".FailResponse(); 
        }

        if (request.File == null || request.File.Length == 0)
        {
            return "Image is required to continue".FailResponse(); 
        }

        var filePath = Path.Combine("../../Images/", request.DriverId + Path.GetExtension(request.File.FileName));

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        return "Success to save image.".SuccessResponse();
    }
}
