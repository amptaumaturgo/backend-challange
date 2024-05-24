using MediatR;

namespace Backend.Shared.CQRS.Events;

public interface IEvent : INotification {  }
 
public interface IEventHandler<in T> : INotificationHandler<T> where T : IEvent { }