namespace Synapse.Presentation.endpoints;

public interface IEndpointDefinition
{
    void MapEndpoints(IEndpointRouteBuilder builder);
}