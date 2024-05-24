namespace Backend.Shared.Domain.Exceptions;

public class InvalidValueObjectException(string message) : Exception(message);