namespace Core.Models;

// Modèle de domaine représentant un utilisateur.
// Remarque : le mot de passe (haché) ne figure PAS ici — il reste interne à l'Infrastructure.
// (cours Clean Architecture : le Core ne dépend pas des détails techniques comme le hachage)
public class User
{
    public int Id { get; set; }
    public string Pseudo { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public DateTime DateInscription { get; set; }
    public decimal SoldeCompte { get; set; }
    public string Statut { get; set; } = "actif";
}
