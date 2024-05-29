using MongoDB.Driver;
using System.Linq.Expressions;

namespace Backend.Infrastructure.Mongo;

internal class MongoDbContext
{
    private readonly IMongoDatabase _database;
    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }

    public IMongoRepository<T> GetRepository<T>(string collectionName) where T : class
    {
        return new MongoRepository<T>(_database, collectionName);
    }
}

public interface IMongoRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id); 
    Task<IEnumerable<T>> FilterByAsync(Expression<Func<T, bool>> filterExpression);
}

public class MongoRepository<T> : IMongoRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
    } 

    public async Task<IEnumerable<T>> FilterByAsync(Expression<Func<T, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).ToListAsync();
    }
}