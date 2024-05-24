using Backend.Shared.Domain.Exceptions;
using System.Net;

namespace Backend.Presentation.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, "An domain exception has occurred");
            await HandleDomainExceptionAsync(context, ex);
        }
        catch (InvalidValueObjectException ex)
        {
            logger.LogError(ex, "An value object exception has occurred");
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            Success = false,
            Message = "Internal Server Error.",
            DetailedMessage = exception.Message
        };

        return context.Response.WriteAsJsonAsync(response);
    }

    private Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            Success = false,
            Message = "Internal Server Error.",
            DetailedMessage = $"Fail on the domain of the application on the entity: {exception.Entity} with Id {exception.Id}"
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}