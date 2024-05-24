namespace Backend.Shared.Domain.Exceptions;

public class DomainException : Exception
{
    public Guid Id { get; private set; }
    public string Entity { get; private set; }

    public DomainException(string entity)
    {
        Entity = entity;
    }

    public DomainException(string message, Guid id, string entity) : base(message)
    {
        Id = id;
        Entity = entity;
    }

    public DomainException(string message, Exception innerException, string entity) : base(message, innerException)
    {
        Entity = entity;
    }
}