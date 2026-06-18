using Core.IGateways;
using Infrastructure.Gateways;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

// Centralise l'enregistrement des services de la couche Infrastructure dans le conteneur DI.
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Repositories : on fournit la chaîne de connexion par le constructeur (comme le DapperRepo du cours).
        services.AddScoped<IUserRepository>(_ => new UserRepository(connectionString));
        services.AddScoped<IGameRepository>(_ => new GameRepository(connectionString));
        services.AddScoped<ICartRepository>(_ => new CartRepository(connectionString));

        // Gateways : implémentent les contrats du Core (cours SOLID/IoC : abstraction -> implémentation).
        services.AddScoped<IUserGateway, UserGateway>();
        services.AddScoped<IGameGateway, GameGateway>();
        services.AddScoped<ICartGateway, CartGateway>();

        return services;
    }
}
