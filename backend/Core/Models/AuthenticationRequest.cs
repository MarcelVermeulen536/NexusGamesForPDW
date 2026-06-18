namespace Core.Models;

// Données envoyées par le frontend pour se connecter (cours : DTO / objet de transfert).
public class AuthenticationRequest
{
    public string Email { get; set; } = string.Empty;
    public string MotDePasse { get; set; } = string.Empty;
}
