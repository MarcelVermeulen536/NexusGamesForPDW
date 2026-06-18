namespace Infrastructure.Models;

// Modèle « base de données » d'un jeu (projection de la requête SQL avec jointures).
// Les genres sont remplis via le multi-mapping de Dapper (cours Dapper : jointures / multi-mapping).
public class Game
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Prix { get; set; }
    public DateTime? DateSortie { get; set; }
    public int? ScoreMetascore { get; set; }
    public string Developpeur { get; set; } = string.Empty;
    public string Plateforme { get; set; } = string.Empty;
    public decimal? PrixReduit { get; set; }
    public List<string> Genres { get; set; } = new();
}
