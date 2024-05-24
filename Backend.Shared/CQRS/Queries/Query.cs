using MediatR;

namespace Backend.Shared.CQRS.Queries;

public abstract class Query<T> : IRequest<QueryResponse<T>> { }

public abstract class QueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, QueryResponse<TResponse>> where TQuery : Query<TResponse>
{
    public abstract Task<QueryResponse<TResponse>> Handle(TQuery request, CancellationToken cancellationToken);
}

public class QueryResponse<T>(T result)
{
    public T Result { get; set; } = result;
}