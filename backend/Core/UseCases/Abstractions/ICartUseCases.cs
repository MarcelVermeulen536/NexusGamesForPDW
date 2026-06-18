using Core.Models;

namespace Core.UseCases.Abstractions;

// Cas d'utilisation « panier / achat / bibliothèque ».
public interface ICartUseCases
{
    void Checkout(int userId, Cart cart);
    IEnumerable<LibraryItem> GetLibrary(int userId);
}
