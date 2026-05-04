using MediatR;
using Synapse.Application.Features.Auth.Contracts;

namespace Synapse.Application.Features.Auth.Commands;

public record RegisterCommand(string Username, string Password) : IRequest<RegisterResponseContract>;