namespace Core.Models;

// Le panier envoyé par le frontend au moment de l'achat (checkout).
public class Cart
{
    public List<CartItem> Items { get; set; } = new();
    public string MethodePaiement { get; set; } = "carte";
}
