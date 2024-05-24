using Backend.Application.Rent.GetAvailablePlans;
using Backend.Application.Rent.GetRentByUser;
using Backend.Application.Rent.RentMotorcycle;
using Backend.Presentation.API.Controllers.Base;
using Backend.Presentation.API.Controllers.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.API.Controllers;

public class RentController(IMediator mediator) : MainController(mediator)
{
    [HttpGet("plans")]
    [Authorize(Roles = "Admin,Driver")]
    public async Task<IActionResult> GetAvailablePlans()
        => await SendQueryAsync<GetAvailablePlansQuery, IEnumerable<GetAvailablePlansQueryResponse>>(new GetAvailablePlansQuery());

    [HttpPost]
    [Authorize(Roles = "Driver")]
    public async Task<IActionResult> RentMotorcycle([FromBody] RentMotorcycleCommand command)
    {
        command.DriverId = Guid.Parse(HttpContext.GetDriverId() ?? string.Empty);
        return await SendCommandAsync(command);
    }

    [HttpGet("user")]
    [Authorize(Roles = "Driver")]
    public async Task<IActionResult> GetRentsByUser()
    {
        var query = new GetRentByUserQuery
        {
            DriverId = Guid.Parse(HttpContext.GetDriverId() ?? string.Empty)
        };

        return await SendQueryAsync<GetRentByUserQuery, GetRentByUserQueryResponse>(query);
    }
}
