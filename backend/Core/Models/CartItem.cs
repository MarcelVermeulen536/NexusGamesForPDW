namespace Core.Models;

// Un article du panier : référence un jeu à acheter.
// Le panier vit côté Angular (service + signals) ; au moment de l'achat,
// le frontend envoie la liste des articles à l'API.
public class CartItem
{
    public int GameId { get; set; }
    public string? Titre { get; set; }
    public decimal Prix { get; set; }
}
