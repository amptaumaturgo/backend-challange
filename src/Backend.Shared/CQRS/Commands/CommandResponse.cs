namespace Backend.Shared.CQRS.Commands;

public record CommandResponse(bool Success, object? Result, IEnumerable<string> Errors)
{
    public CommandResponse(IEnumerable<string> errors) : this(false, null, errors) { }

    public bool Success { get; private set; } = Success;
    public object? Result { get; private set; } = Result;
    public IEnumerable<string> Errors { get; private set; } = Errors;
}