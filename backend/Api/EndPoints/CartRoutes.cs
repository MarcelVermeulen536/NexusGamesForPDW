using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

// Routes liées à l'achat et à la bibliothèque (Minimal API).
// Auth simplifiée (pas de JWT) : l'identifiant de l'utilisateur est passé dans l'URL.
public static class CartRoutes
{
    public static void MapCartRoutes(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/cart");

        // Achat du panier : enregistre les achats (le trigger SQL alimente la bibliothèque)
        group.MapPost("/checkout/{userId:int}", (int userId, Cart cart, ICartUseCases cartUseCases) =>
        {
            cartUseCases.Checkout(userId, cart);
            return Results.Ok(new { message = "Achat effectué avec succès." });
        });

        // Bibliothèque de l'utilisateur (jeux possédés)
        group.MapGet("/library/{userId:int}", (int userId, ICartUseCases cartUseCases) =>
            Results.Ok(cartUseCases.GetLibrary(userId)));
    }
}
