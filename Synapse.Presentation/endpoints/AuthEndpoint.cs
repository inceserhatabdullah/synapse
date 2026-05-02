using Synapse.Infrastructure.Services;
using Synapse.Presentation.DTOs;

namespace Synapse.Presentation.endpoints;

public class AuthEndpoint: IEndpointDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/auth");

        group.MapPost("/login", Login);
    }

     private IResult Login(LoginRequest request, IAuthService authService)
    {
        var token = authService.GenerateToken(request.username, Guid.NewGuid().ToString());
        return Results.Ok(new { Token = token });
    }
}