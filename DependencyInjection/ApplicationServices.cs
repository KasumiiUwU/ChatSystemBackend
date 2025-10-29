using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Application.Services;
using ChatSystemBackend.Infrastructure.Repository;
using ChatSystemBackend.Utils;
using ChatSystemBackend.Utils.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace ChatSystemBackend.DependencyInjection;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddServices();
        services.AddDatabaseService(configuration);
        services.AddAuthenticationAuthorized(configuration);
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddSwaggerDocumentation();
        services.AddCors();
        return services;
    }

    //Inject các services
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<JwtSecurityTokenHandler>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped<IConversationParticipantService, ConversationParticipantService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddSingleton<IPasswordUtils, PasswordUtils>();
        services.AddSignalR();
        
        return services;
    }

    private static IServiceCollection AddAuthenticationAuthorized(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    },
                    
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken) &&
                            context.HttpContext.Request.Path.StartsWithSegments("/chathub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                    
                };
            });
        return services;
    }

    private static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatSystem", Version = "v1" });

            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

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
        services.AddScoped(typeof(IApplicationRepository<>), typeof(ApplicationRepository<>));

        return services;
    }

    private static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(o => o.AddDefaultPolicy(
            builder =>builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() ));
        return services;
    }
}