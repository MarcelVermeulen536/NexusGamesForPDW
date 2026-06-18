using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

// Contrat d'accès aux données « achat / bibliothèque ».
public interface ICartRepository
{
    // Enregistre l'achat des jeux dans une transaction (cours Dapper : transactions).
    void Checkout(int userId, IEnumerable<int> gameIds, string methodePaiement);

    // Récupère la bibliothèque (jeux possédés) d'un utilisateur.
    IEnumerable<LibraryItem> GetLibrary(int userId);
}
