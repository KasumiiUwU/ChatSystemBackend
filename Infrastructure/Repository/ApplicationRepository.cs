using System.Linq.Expressions;
using ChatSystemBackend.Application.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ChatSystemBackend.Infrastructure.Repository;

public class ApplicationRepository<TEntity> : IApplicationRepository<TEntity> where TEntity : class
{
    private IMongoCollection<TEntity> _collection;

    public ApplicationRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<TEntity>(collectionName);
    }

    
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var result = await _collection.FindAsync(Builders<TEntity>.Filter.Empty);
        return result.ToList();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
        int? pageIndex = null, int? pageSize = null)
    {
        
        IQueryable<TEntity> query = _collection.AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (pageIndex != null && pageSize != null)
        {
            query = query.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }

    public Task<TEntity> GetByIdAsync(object id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        return _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public Task UpdateAsync(object id, TEntity entity)
    {
        var filter = Builders<TEntity>.Filter.Eq("id", id);
        return _collection.ReplaceOneAsync(filter, entity);
    }

    public Task DeleteAsync(object id)
    {
        var filter = Builders<TEntity>.Filter.Eq("id", id);
        return _collection.DeleteOneAsync(filter);
    }

    public async Task<bool> ExistAsync(object id)
    {
        var filter = Builders<TEntity>.Filter.Eq("id", id);
        var count = await _collection.CountDocumentsAsync(filter);
        return count > 0;
    }
}