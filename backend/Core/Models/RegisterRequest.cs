namespace Core.Models;

// Données envoyées par le frontend pour créer un compte (cours : DTO / objet de transfert).
public class RegisterRequest
{
    public string Pseudo { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MotDePasse { get; set; } = string.Empty;
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
}
