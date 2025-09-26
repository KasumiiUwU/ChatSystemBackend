using ChatSystemBackend.Domain.Interfaces;
using MongoDB.Driver;

namespace ChatSystemBackend.Infrastructure.Repository;

public class UnitOfWork: IUnitOfWork
{
    private readonly IMongoDatabase _database;

    public UnitOfWork(IMongoDatabase database)
    {
        _database = database;
    }


    public IApplicationRepository<T> Repository<T>(string collectionName) where T : class
    {
        return new ApplicationRepository<T>(_database, collectionName);
    }

}