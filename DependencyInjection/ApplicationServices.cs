using System.IdentityModel.Tokens.Jwt;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Infrastructure.Repository;
using MongoDB.Driver;

namespace ChatSystemBackend.DependencyInjection;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices();
        services.AddDatabaseService(configuration);
        services.AddHttpContextAccessor();
        return services;
    }

    //Inject các services
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<JwtSecurityTokenHandler>();
        return services;
    }

    //Khởi tạo database
    private static IServiceCollection AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ChatSystemDB");
        var mongoUrl = MongoUrl.Create(connectionString);
        var databaseName = mongoUrl.DatabaseName;
        
        var mongoClient = new MongoClient(mongoUrl);
        services.AddSingleton<IMongoDatabase>(mongoClient.GetDatabase(databaseName));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IApplicationRepository<>),  typeof(ApplicationRepository<>));
        
        return services;
    }
    
}