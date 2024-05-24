using Backend.Shared.CQRS.Events;

namespace Backend.Application.Motorcycle.Events;

public record CreateMotorcycleEvent(int Year, string Plate, string Model) : IEvent;