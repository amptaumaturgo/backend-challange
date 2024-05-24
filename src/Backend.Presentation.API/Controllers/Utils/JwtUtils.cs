using System.Security.Claims;

namespace Backend.Presentation.API.Controllers.Utils;

public static class JwtUtils
{
    public static string? GetDriverId(this HttpContext httpContext)
    {
        return httpContext?.User?.Claims.FirstOrDefault(c => c.Type == "DriverId")?.Value;
    }

}