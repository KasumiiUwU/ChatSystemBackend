using MongoDB.Driver;

namespace ChatSystemBackend.DependencyInjection;

public class ApplicationServices
{
    public IServiceCollection AddApplicationServices(IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    private static IServiceCollection AddDatabaseService(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ChatSystemDB");
        var mongoUrl = MongoUrl.Create(connectionString);
        var databaseName = mongoUrl.DatabaseName;
        
        var mongoClient = new MongoClient(mongoUrl);
        services.AddSingleton<IMongoDatabase>(mongoClient.GetDatabase(databaseName));
        
        
        return services;
    }
    
}