namespace Synapse.Application.Features.Auth.Records;

public record JwtRecord(string Secret, string ExpiresIn);
