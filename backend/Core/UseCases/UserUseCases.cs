using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

// Logique métier liée aux utilisateurs.
// (cours SOLID/IoC : la dépendance IUserGateway est INJECTÉE par le constructeur,
//  le use case dépend d'une abstraction, pas d'une implémentation concrète)
public class UserUseCases : IUserUseCases
{
    private readonly IUserGateway _userGateway;

    public UserUseCases(IUserGateway userGateway)
    {
        _userGateway = userGateway;
    }

    // Connexion : délègue la vérification des identifiants au gateway.
    public User? Login(AuthenticationRequest request)
    {
        return _userGateway.Authenticate(request.Email, request.MotDePasse);
    }

    // Inscription : règles métier de validation, puis création.
    public User Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Pseudo)
            || string.IsNullOrWhiteSpace(request.Email)
            || string.IsNullOrWhiteSpace(request.MotDePasse))
        {
            // cours C# : lever une exception adaptée plutôt que retourner un code d'erreur
            throw new ArgumentException("Le pseudo, l'email et le mot de passe sont obligatoires.");
        }

        if (request.MotDePasse.Length < 6)
        {
            throw new ArgumentException("Le mot de passe doit contenir au moins 6 caractères.");
        }

        if (_userGateway.ExistsByEmailOrPseudo(request.Email, request.Pseudo))
        {
            throw new InvalidOperationException("Un compte existe déjà avec cet email ou ce pseudo.");
        }

        return _userGateway.Register(request);
    }
}
