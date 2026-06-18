using Core.UseCases;
using Core.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

// Centralise l'enregistrement des services de la couche Core dans le conteneur d'injection
// de dépendances (cours : injection de dépendances / IoC).
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // On associe chaque abstraction à son implémentation.
        // AddScoped : une instance par requête HTTP.
        services.AddScoped<IUserUseCases, UserUseCases>();
        services.AddScoped<IGameUseCases, GameUseCases>();
        services.AddScoped<ICartUseCases, CartUseCases>();
        return services;
    }
}
