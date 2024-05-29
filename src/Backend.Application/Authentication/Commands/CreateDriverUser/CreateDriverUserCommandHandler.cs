using Backend.Application.Authentication.Commands.Login;
using Backend.Domain.Repositories.Driver;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Commands;
using Backend.Shared.Data;
using Microsoft.AspNetCore.Identity;

namespace Backend.Application.Authentication.Commands.CreateDriverUser;

public class CreateDriverUserCommandHandler(UserManager<IdentityUser> userManager, IDriverRepository driverRepository, IUnitOfWork unitOfWork) : CommandHandler<CreateDriverUserCommand>
{
    public override async Task<CommandResponse> Handle(CreateDriverUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = request.Validate(new CreateDriverUserCommandValidator(), request);

        if (!validationResult.IsValid)
            return validationResult.FailResponse();

        if (await driverRepository.ExistentCnpj(request.Cnpj))
            return "Cnpj existent, please try with other Cnpj.".FailResponse();
         
        IdentityUser identityUser = new()
        {
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(identityUser, request.Password);


        if (!result.Succeeded) return result.Errors.FailResponse(result.Errors.Select(x => x.Description).ToArray());

        await userManager.AddToRoleAsync(identityUser, "Driver");

        var driver = new Domain.Entities.Driver(request.Name, request.Cnpj, request.DriverLicenseNumber,
            request.BirthDate, request.DriverLicenseType, Guid.Parse(identityUser.Id));

        driverRepository.Add(driver);

        await unitOfWork.CommitAsync();
         
        return "Success to create your access.".SuccessResponse();
    }
}