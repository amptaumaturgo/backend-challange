using Backend.Shared.Domain.Exceptions;

namespace Backend.Domain.Entities.ValueObject;

public class Document
{
    public string Value { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;

    public Document(string value, string type)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidValueObjectException("Document cannot be empty.");
        }

        Value = value;
        Type = type;
    }

    public Document() { }

    public override string ToString()
    {
        return Type + " " + Value;
    }


    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Document)obj;
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}