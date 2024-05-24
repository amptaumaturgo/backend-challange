using Backend.Application;
using Backend.Presentation.API.Middlewares;

namespace Backend.Presentation.API.Configurations;

public static class ApiConfigurations
{
    public static void AddApiConfigurations(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
    }

    public static void ConfigureApiConfigurations(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication(); 
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}