namespace ChatSystemBackend.Domain.Interfaces;

public interface IUnitOfWork
{ 
    IApplicationRepository<TEntity> Repository<TEntity>(string collectionName) where TEntity : class;
    
}