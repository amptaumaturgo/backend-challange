using Backend.Shared.CQRS.Commands;
using Backend.Shared.CQRS.Queries;
using FluentValidation.Results; 

namespace Backend.Shared.CQRS.Base;

public static class ResponseExtensions
{
    public static QueryResponse<T> SuccessQueryResponse<T>(this T response)
    {
        return new QueryResponse<T>(response);
    }

    public static CommandResponse SuccessResponse<T>(this T? response)
    {
        return new CommandResponse(true, response, []);
    }

    public static CommandResponse FailResponse<T>(this T? response, params string[] errors)
    {
        return new CommandResponse(false, response, errors);
    }

    public static CommandResponse FailResponse(this IEnumerable<string> errors)
    {
        return new CommandResponse(false, null, errors);
    }

    public static CommandResponse FailResponse(this List<string> errors)
    {
        return new CommandResponse(false, null, errors);
    }

    public static CommandResponse FailResponse(this string[] errors)
    {
        return new CommandResponse(false, null, errors);
    }

    public static CommandResponse FailResponse(this ValidationResult validationResult)
    {
        return new CommandResponse(false, null, validationResult.Errors.Select(x => x.ErrorMessage));
    }

    public static CommandResponse FailResponse(this string response)
    {
        return new CommandResponse(false, null, new List<string>
        {
            response
        });
    }
}