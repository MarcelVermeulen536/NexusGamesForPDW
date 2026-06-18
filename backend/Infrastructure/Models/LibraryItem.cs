namespace Infrastructure.Models;

// Modèle « base de données » d'une entrée de bibliothèque (jointure bibliotheque + jeux).
public class LibraryItem
{
    public int GameId { get; set; }
    public string Titre { get; set; } = string.Empty;
    public DateTime? DateAcquisition { get; set; }
    public int TempsJeuHeures { get; set; }
    public int CompletionPourcentage { get; set; }
}
