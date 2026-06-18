namespace Infrastructure.Models;

// Modèle « base de données » d'un utilisateur (projection d'une ligne de la table utilisateurs).
// Contrairement au modèle de domaine Core.Models.User, il inclut le mot de passe haché :
// c'est un détail technique qui ne doit PAS remonter dans le Core (cours Clean Architecture).
public class User
{
    public int Id { get; set; }
    public string Pseudo { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MotDePasse { get; set; } = string.Empty;
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public DateTime DateInscription { get; set; }
    public decimal SoldeCompte { get; set; }
    public string Statut { get; set; } = "actif";
}
