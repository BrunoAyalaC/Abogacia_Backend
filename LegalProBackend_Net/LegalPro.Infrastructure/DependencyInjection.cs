using LegalPro.Application.Common.Interfaces;
using LegalPro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

namespace LegalPro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DATABASE_URL (Railway) o ConnectionStrings:DefaultConnection (appsettings)
        var connectionString = configuration["DATABASE_URL"]
                               ?? configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("ConnectionString no configurada.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IGeminiService, LegalPro.Infrastructure.Services.GeminiService>();
        services.AddScoped<ISimulationAI>(provider => provider.GetRequiredService<IGeminiService>());
        services.AddScoped<ISimulationService, LegalPro.Infrastructure.Services.SimulationService>();
        services.AddScoped<IJwtService, LegalPro.Infrastructure.Services.JwtService>();

        // Resiliencia de IA: Polly v8 Pipeline para Gemini
        services.AddResiliencePipeline("gemini-pipeline", builder =>
        {
            builder.AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder().Handle<Exception>()
            })
            .AddTimeout(TimeSpan.FromSeconds(45));
        });

        return services;
    }
}
