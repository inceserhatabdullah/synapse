using MediatR;
using Microsoft.AspNetCore.Mvc;
using Synapse.Application.Features.Auth.Commands;
using Synapse.Application.Features.Auth.Contracts;

namespace Synapse.Presentation.Controllers;

public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestContract request)
    {
        var command = new RegisterCommand(request.username, request.password);
        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestContract request)
    {
        var command = new LoginCommand(request.username, request.password);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}