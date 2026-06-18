using Core.Models;

namespace Core.UseCases.Abstractions;

// Cas d'utilisation « utilisateur » (logique métier : inscription, connexion).
public interface IUserUseCases
{
    User? Login(AuthenticationRequest request);
    User Register(RegisterRequest request);
}
