using Backend.Shared.CQRS.Commands;
using Backend.Shared.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Backend.Presentation.API.Controllers.Base;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public abstract class MainController(IMediator mediator) : ControllerBase
{
    public async Task<IActionResult> SendCommandAsync<T>(T command) where T : Command
    {
        var response = await mediator.Send(command); 

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    protected async Task<IActionResult> SendQueryAsync<TQuery, TResponse>(TQuery query) where TQuery : Query<TResponse>
    {
        var result = await  mediator.Send(query);

        if (result.Result == null)
        {
            return NotFound(new
            {
                Result = "Not found"
            });  
        }

        if (result.Result is IEnumerable enumerable && !enumerable.GetEnumerator().MoveNext())
        {
            return NoContent(); 
        }

        return Ok(result);
    }
}