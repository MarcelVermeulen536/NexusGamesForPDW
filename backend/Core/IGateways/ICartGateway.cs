using Core.Models;

namespace Core.IGateways;

// Contrat d'accès aux données liées à l'achat et à la bibliothèque.
public interface ICartGateway
{
    // Enregistre l'achat des jeux du panier (insertion dans `achats`).
    // L'insertion se fait dans une transaction (cours Dapper : transactions).
    // Le trigger SQL ajoute ensuite automatiquement les jeux à la bibliothèque.
    void Checkout(int userId, Cart cart);

    // Récupère la bibliothèque (jeux possédés) d'un utilisateur.
    IEnumerable<LibraryItem> GetLibrary(int userId);
}
