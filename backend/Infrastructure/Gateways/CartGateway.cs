using Core.IGateways;
using Infrastructure.Repositories.Abstractions;
using CoreModels = Core.Models;

namespace Infrastructure.Gateways;

// Implémente Core.IGateways.ICartGateway (achat + bibliothèque).
public class CartGateway : ICartGateway
{
    private readonly ICartRepository _cartRepository;

    public CartGateway(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public void Checkout(int userId, CoreModels.Cart cart)
    {
        // On extrait les identifiants de jeux du panier (cours C# : LINQ Select)
        var gameIds = cart.Items.Select(item => item.GameId);
        _cartRepository.Checkout(userId, gameIds, cart.MethodePaiement);
    }

    public IEnumerable<CoreModels.LibraryItem> GetLibrary(int userId)
    {
        return _cartRepository.GetLibrary(userId).Select(item => new CoreModels.LibraryItem
        {
            GameId = item.GameId,
            Titre = item.Titre,
            DateAcquisition = item.DateAcquisition,
            TempsJeuHeures = item.TempsJeuHeures,
            CompletionPourcentage = item.CompletionPourcentage,
        });
    }
}
