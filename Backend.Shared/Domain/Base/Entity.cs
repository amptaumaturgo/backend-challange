namespace Backend.Shared.Domain.Base;

public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
}

public interface IAggregateRoot { }