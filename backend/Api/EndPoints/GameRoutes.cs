using Core.UseCases.Abstractions;

namespace Api.EndPoints;

// Routes du catalogue de jeux (Minimal API).
public static class GameRoutes
{
    public static void MapGameRoutes(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/games");

        // Liste de tous les jeux
        group.MapGet("/", (IGameUseCases gameUseCases) =>
            Results.Ok(gameUseCases.GetAll()));

        // Détail d'un jeu ; si introuvable, le UseCase lève une exception
        // traduite en 404 par le middleware global.
        group.MapGet("/{id:int}", (int id, IGameUseCases gameUseCases) =>
            Results.Ok(gameUseCases.GetById(id)));
    }
}
