using MediatR;

namespace Backend.Shared.CQRS.Commands;

public abstract class CommandHandler<T> : IRequestHandler<T, CommandResponse> where T : Command
{
    public abstract Task<CommandResponse> Handle(T request, CancellationToken cancellationToken);
}