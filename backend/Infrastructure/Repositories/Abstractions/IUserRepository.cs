using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

// Contrat d'accès aux données « utilisateur » (cours SOLID : on dépend d'une abstraction).
public interface IUserRepository
{
    User? GetByEmail(string email);
    bool ExistsByEmailOrPseudo(string email, string pseudo);
    int Insert(User user);
}
