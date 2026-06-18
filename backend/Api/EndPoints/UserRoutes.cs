using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

// Routes liées aux utilisateurs (Minimal API).
// Les endpoints ne touchent JAMAIS la base : ils appellent les UseCases (consigne respectée).
public static class UserRoutes
{
    public static void MapUserRoutes(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users");

        // Inscription : IUserUseCases est injecté automatiquement (cours : injection de dépendances)
        group.MapPost("/register", (RegisterRequest request, IUserUseCases userUseCases) =>
        {
            User user = userUseCases.Register(request);
            return Results.Created($"/api/users/{user.Id}", user);
        });

        // Connexion : renvoie l'utilisateur si les identifiants sont bons, sinon 401
        group.MapPost("/login", (AuthenticationRequest request, IUserUseCases userUseCases) =>
        {
            User? user = userUseCases.Login(request);
            return user is null ? Results.Unauthorized() : Results.Ok(user);
        });
    }
}
