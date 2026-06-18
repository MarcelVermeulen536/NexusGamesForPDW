namespace Core.Models;

// Modèle de domaine représentant un jeu du catalogue.
// (cours POO : une classe encapsule les données d'une entité métier)
// Les champs Developpeur / Plateforme / Genres proviennent de jointures SQL (cours Dapper : multi-mapping / joins).
public class Game
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Prix { get; set; }
    public DateTime? DateSortie { get; set; }
    public int? ScoreMetascore { get; set; }

    // Données issues des tables liées (developpeurs, plateformes, jeux_genres/genres)
    public string Developpeur { get; set; } = string.Empty;
    public string Plateforme { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new();

    // Prix promotionnel si une promotion est active pour ce jeu (sinon null)
    public decimal? PrixReduit { get; set; }
}
