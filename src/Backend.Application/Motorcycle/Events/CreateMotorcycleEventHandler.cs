using Backend.Communication.Base.Interfaces;
using Backend.Shared.CQRS.Events;

namespace Backend.Application.Motorcycle.Events;

public class CreateMotorcycleEventHandler(IMessagePublisher messagePublisher) : IEventHandler<CreateMotorcycleEvent>
{
    public async Task Handle(CreateMotorcycleEvent notification, CancellationToken cancellationToken)
    {
        await messagePublisher.Publish("Rent", "create.motorcycle", notification);
    }
}