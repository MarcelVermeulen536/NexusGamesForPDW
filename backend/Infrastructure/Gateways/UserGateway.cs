using Core.IGateways;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;
using CoreModels = Core.Models;
using DbModels = Infrastructure.Models;

namespace Infrastructure.Gateways;

// Implémente le contrat Core.IGateways.IUserGateway (cours : DIP — l'adaptateur concret de la couche Infra).
// Rôle : faire le pont entre le repository (données brutes + mot de passe) et le modèle de domaine,
// et encapsuler le hachage du mot de passe (Base64Utils).
public class UserGateway : IUserGateway
{
    private readonly IUserRepository _userRepository;

    public UserGateway(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public CoreModels.User? Authenticate(string email, string motDePasse)
    {
        DbModels.User? dbUser = _userRepository.GetByEmail(email);
        if (dbUser is null)
        {
            return null;
        }

        // Vérification du mot de passe haché (le hachage reste un détail de l'Infrastructure)
        if (!Base64Utils.Verify(motDePasse, dbUser.MotDePasse))
        {
            return null;
        }

        return ToDomain(dbUser);
    }

    public bool ExistsByEmailOrPseudo(string email, string pseudo)
    {
        return _userRepository.ExistsByEmailOrPseudo(email, pseudo);
    }

    public CoreModels.User Register(CoreModels.RegisterRequest request)
    {
        // Le mot de passe est haché AVANT d'être confié au repository (jamais stocké en clair)
        var dbUser = new DbModels.User
        {
            Pseudo = request.Pseudo,
            Email = request.Email,
            MotDePasse = Base64Utils.Hash(request.MotDePasse),
            Nom = request.Nom,
            Prenom = request.Prenom,
        };

        int newId = _userRepository.Insert(dbUser);
        dbUser.Id = newId;
        return ToDomain(dbUser);
    }

    // Mappe le modèle « base de données » vers le modèle de domaine (sans le mot de passe).
    private static CoreModels.User ToDomain(DbModels.User dbUser) => new()
    {
        Id = dbUser.Id,
        Pseudo = dbUser.Pseudo,
        Email = dbUser.Email,
        Nom = dbUser.Nom,
        Prenom = dbUser.Prenom,
        DateInscription = dbUser.DateInscription,
        SoldeCompte = dbUser.SoldeCompte,
        Statut = dbUser.Statut,
    };
}
