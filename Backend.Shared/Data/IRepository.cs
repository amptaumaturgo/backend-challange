using Backend.Shared.Domain.Base;

namespace Backend.Shared.Data;

public interface IRepository<T> where T : IAggregateRoot { }