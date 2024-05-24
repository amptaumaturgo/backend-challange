using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend.Shared.Extensions;
using Microsoft.Extensions.Options;
using System.Text;
using Backend.Domain.Repositories;

namespace Backend.Application.Authentication.Commands.Login;

public class LoginCommandHandler(SignInManager<IdentityUser> signInManager, IDriverRepository driverRepository, UserManager<IdentityUser> userManager, IOptions<AuthenticationSettings> authenticationOptions) : CommandHandler<LoginCommand>
{
    public override async Task<CommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = request.Validate(new LoginCommandValidator(), request);

        if (!validationResult.IsValid)
            return validationResult.FailResponse();

        var loginResult = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

        if (!loginResult.Succeeded) return "Invalid login attempt.".FailResponse();
         
        var token = await GenerateToken(request.Email);

        return new LoginCommandResponse(token).SuccessResponse();
    }
    private async Task<string> GenerateToken(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
          
        var claims = await userManager.GetClaimsAsync(user!);

        var identityClaims = await GetUserClaims(claims, user!);
        var encodedToken = WriteToken(identityClaims);

        return encodedToken;
    }

    private async Task<ClaimsIdentity> GetUserClaims(IList<Claim> claims, IdentityUser user)
    {
        var userRoles = await userManager.GetRolesAsync(user);

        var driver = await driverRepository.GetByUserId(Guid.Parse(user!.Id));
        if(driver != null) claims.Add(new Claim("DriverId", driver.Id.ToString()));

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64));

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }


        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private string WriteToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authenticationOptions.Value.Secret));

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(authenticationOptions.Value.ExpirationTime),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }
}