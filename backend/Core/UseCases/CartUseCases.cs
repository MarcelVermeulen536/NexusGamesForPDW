using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

// Logique métier liée à l'achat et à la bibliothèque.
public class CartUseCases : ICartUseCases
{
    // Méthodes de paiement autorisées (doivent correspondre à l'ENUM de la table achats).
    private static readonly string[] MethodesPaiementValides =
        { "carte", "paypal", "bitcoin", "portefeuille" };

    private readonly ICartGateway _cartGateway;

    public CartUseCases(ICartGateway cartGateway)
    {
        _cartGateway = cartGateway;
    }

    public void Checkout(int userId, Cart cart)
    {
        // Règles métier : le panier ne peut pas être vide
        if (cart.Items is null || cart.Items.Count == 0)
        {
            throw new ArgumentException("Le panier est vide.");
        }

        // ... et la méthode de paiement doit être valide
        if (!MethodesPaiementValides.Contains(cart.MethodePaiement))
        {
            throw new ArgumentException($"Méthode de paiement invalide : {cart.MethodePaiement}.");
        }

        _cartGateway.Checkout(userId, cart);
    }

    public IEnumerable<LibraryItem> GetLibrary(int userId)
    {
        return _cartGateway.GetLibrary(userId);
    }
}
