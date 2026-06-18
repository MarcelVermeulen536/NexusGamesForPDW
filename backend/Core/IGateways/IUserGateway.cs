using Core.Models;

namespace Core.IGateways;

// Contrat d'accès aux données « utilisateur ».
// (cours SOLID - Inversion des dépendances : le Core dépend d'une abstraction,
//  l'implémentation concrète (Dapper) se trouve dans l'Infrastructure)
public interface IUserGateway
{
    // Vérifie les identifiants ; le hachage du mot de passe est fait dans l'implémentation.
    User? Authenticate(string email, string motDePasse);

    // Indique si un email ou un pseudo est déjà utilisé.
    bool ExistsByEmailOrPseudo(string email, string pseudo);

    // Crée l'utilisateur (mot de passe haché dans l'implémentation) et le retourne.
    User Register(RegisterRequest request);
}
