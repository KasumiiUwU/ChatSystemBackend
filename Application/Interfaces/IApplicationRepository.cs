using System.Linq.Expressions;

namespace ChatSystemBackend.Application.Interfaces;

public interface IApplicationRepository<TEntity> where TEntity : class
{ 
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        int? pageIndex=null, int? pageSize=null);

    Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
    
    Task<TEntity> GetByIdAsync(object id);
    Task InsertAsync(TEntity entity);
    Task UpdateAsync(object id, TEntity entity);
    Task DeleteAsync(object id);
    Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);

}