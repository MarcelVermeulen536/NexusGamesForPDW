namespace Core.Models;

// Une entrée de la bibliothèque d'un utilisateur (table bibliotheque).
// Alimentée automatiquement par le trigger SQL après un achat « complete ».
public class LibraryItem
{
    public int GameId { get; set; }
    public string Titre { get; set; } = string.Empty;
    public DateTime? DateAcquisition { get; set; }
    public int TempsJeuHeures { get; set; }
    public int CompletionPourcentage { get; set; }
}
