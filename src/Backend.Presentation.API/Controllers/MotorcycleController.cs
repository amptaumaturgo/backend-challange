using Backend.Application.Motorcycle.Commands.CreateMotorcycle;
using Backend.Application.Motorcycle.Commands.RemoveMotorcycle;
using Backend.Application.Motorcycle.Commands.UpdateMotorcyclePlate;
using Backend.Application.Motorcycle.Queries.GetAll;
using Backend.Application.Motorcycle.Queries.GetByPlate;
using Backend.Presentation.API.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.API.Controllers;

public class MotorcycleController(IMediator mediator) : MainController(mediator)
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateMotorcycle(CreateMotorcycleCommand command) => await SendCommandAsync(command);

    [HttpGet]
    [Authorize(Roles = "Admin,Driver")]
    public async Task<IActionResult> GetAll() => await SendQueryAsync<GetAllQuery, IEnumerable<GetAllQueryResponse>>(new GetAllQuery());

    [HttpGet("plate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByPlate([FromQuery] GetByPlateQuery query) => await SendQueryAsync<GetByPlateQuery, GetByPlateQueryResponse>(query);

    [HttpPatch("change-plate/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateMotorcyclePlate([FromRoute] Guid id, [FromBody] UpdateMotorcyclePlateCommand command)
    {
        command.Id = id;
        return await SendCommandAsync(command);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveMotorcycle([FromRoute] Guid id)
    {
        var command = new RemoveMotorcycleCommand
        {
            Id = id
        };

        return await SendCommandAsync(command);
    }

}